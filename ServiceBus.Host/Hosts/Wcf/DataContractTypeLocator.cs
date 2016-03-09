namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Contracts;

    public class DataContractTypeLocator
    {
        private static readonly List<Assembly> _registeredAssemblies = new List<Assembly>();
        private static List<Type> knownContractTypes = new List<Type>();

        private static readonly object _lock = new object();


        public static IEnumerable<Type> GetTypes(ICustomAttributeProvider prov)
        {
            if (knownContractTypes != null && knownContractTypes.Any())
                return knownContractTypes;

            lock (_lock)
            {
                if (knownContractTypes != null && knownContractTypes.Any())
                    return knownContractTypes;

                var entryAssembly = Assembly.GetEntryAssembly();

                var assemblies = _registeredAssemblies;

                if (entryAssembly != null)
                {
                    assemblies.AddRange(entryAssembly
                        .GetReferencedAssemblies()
                        .Where(a => !a.FullName.StartsWith("System."))
                        .Where(a => !a.FullName.StartsWith("Microsoft."))
                        .Where(a => !a.Name.Equals("System"))
                        .Where(a => !a.FullName.Contains("mscorlib"))
                        .Where(a => !a.FullName.Contains("vshost"))
                        .Where(a => !a.Name.Equals("Microsoft"))
                        .Select(Assembly.Load));

                    _registeredAssemblies.Add(entryAssembly);
                }

                knownContractTypes = GetAllCommandTypes(assemblies);
                return knownContractTypes;
            }
        }


        private static List<Type> GetAllCommandTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.GetTypes()
                             .Where(t => t == typeof(Command) ||
                                         t.IsSubclassOf(typeof(Command)))).ToList();
        }


        public static void RegisterAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (_registeredAssemblies.Contains(assembly))
                return;

            _registeredAssemblies.Add(assembly);
        }
    }
}
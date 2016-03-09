namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml;

    internal class WcfDataContractResolver : DataContractResolver
    {
        private readonly IEnumerable<Type> _derivedTypes;


        public WcfDataContractResolver(Type type)
        {
            _derivedTypes = GetDerivedTypes(type);
        }

        public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (!knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace))
            {
                var dictionary = new XmlDictionary();
                typeName = dictionary.Add(dataContractType.FullName);
                typeNamespace = dictionary.Add(dataContractType.Assembly.FullName);
            }

            return true;
        }

        public override Type ResolveName(
                                        string typeName, 
                                        string typeNamespace,
                                        Type declaredType,
                                        DataContractResolver knownTypeResolver)
        {
            if (string.IsNullOrWhiteSpace(typeNamespace))
                return null;

            var commandNamespace = string.Empty;

            var lastIndexOfSlash = typeNamespace.LastIndexOf("/", StringComparison.InvariantCulture) + 1;

            if (typeNamespace.Length > lastIndexOfSlash)
                commandNamespace = typeNamespace.Substring(lastIndexOfSlash);

            var commandType = commandNamespace + "." + typeName;

            return _derivedTypes.FirstOrDefault(t => t.FullName.Contains(commandType));
        }


        private static IEnumerable<Type> GetDerivedTypes(Type type)
        {
            var assemblies = Assembly.GetEntryAssembly()
                                     .GetReferencedAssemblies()
                                     .Where(a => !a.FullName.StartsWith("System."))
                                     .Where(a => !a.FullName.StartsWith("Microsoft."))
                                     .Where(a => !a.Name.Equals("System"))
                                     .Where(a => !a.FullName.Contains("mscorlib"))
                                     .Where(a => !a.FullName.Contains("vshost"))
                                     .Where(a => !a.Name.Equals("Microsoft"))
                                     .Select(Assembly.Load);

            return assemblies.SelectMany(assembly => assembly.GetTypes()
                                                             .Where(t => t == type || t.IsSubclassOf(type)));
        }
    }
}
namespace ServiceBus.Hosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Instrumentation;
    using System.Reflection;
    using CommandService;
    using Infrastructure;
    using Infrastructure.Validation;
    using Wcf;
    using Wcf.Configuration;
    using Infrastructure.Configuration;

    internal class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly IAssemblyLoader _assemblyLoader;

        private readonly IDictionary<Type, object> _cache = new Dictionary<Type, object>();


        public DefaultDependencyResolver()
            : this(new AssemblyLoader())
        {

        }

        internal DefaultDependencyResolver(IAssemblyLoader assemblyLoader)
        {
            if (assemblyLoader == null)
                throw new ArgumentNullException(nameof(assemblyLoader));

            _assemblyLoader = assemblyLoader;
        }


        public object GetService(Type type)
        {
            if (!_cache.ContainsKey(type))
                _cache[type] = GetServiceInternal(type);

            return _cache[type];
        }


        public IEnumerable<object> GetServices(Type type)
        {
            var service = GetService(type);

            return service != null ? new List<object> { service } : new List<object>();
        }


        private object GetServiceInternal(Type type)
        {
            if (type == typeof(ICommandProcessor))
                return new CommandProcessor();

            if (type == typeof(IServiceErrorHandler))
                return new ConsoleServiceErrorHandler();

            //if (type == typeof(ICServiceBusLogger))
            //    return new SilentCServiceBusLogger();

            if (type == typeof(ICommandHandlerFactory))
                return new CommandHandlerFactory();

            if (type == typeof(ICommandHandlerInvoker))
                return new CommandHandlerInvoker();

            if (type == typeof(IHttpBasicConfiguration))
                return new HttpBasicConfiguration();

            if (type == typeof(IHost))
                return new MsmqServiceHost();

            if (type == typeof(IMsmqConfiguration))
                return new MsmqConfiguration();

            if (type == typeof(IConfigurationReader))
                return new AppConfigReader();

            if (type == typeof(ICommandValidator))
                return new AnnotationCommandValidator();

            if (type == typeof(ICommandProcessorInspector))
                return new SilentCommandProcessorInspector();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                return CreateHandlerInstance(FindHandlerType(type));

            return null;
        }

        private static object CreateHandlerInstance(Type target)
        {
            try
            {
                return Activator.CreateInstance(target);
            }
            catch (Exception e)
            {
                if (target.GetConstructors().All(c => c.GetParameters().Any()))
                    throw new ResolveDependencyException(
                        $"The {target.FullName} does not have a default constructor and can not be created. Either create a default constructor or use the SetDependencyResolver method of the host config in your startup class to take control of the initialization.",
                        e);

                throw new ResolveDependencyException(
                    $"When creating a the {target.FullName} there was a error, please see inner exception for more information.",
                    e);
            }
        }


        private Type FindHandlerType(Type type)
        {
            var assemblies = GetAssemblies();

            var target = assemblies.SelectMany(a => a.GetTypes())
                                                   .FirstOrDefault(t => t.GetInterfaces().Contains(type));

            if (target == null)
                throw new InstanceNotFoundException(
                    $"Can't find '{type.Name}' for command of type '{type.GenericTypeArguments.Single().Name}'");

            return target;
        }


        private IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = _assemblyLoader.GetEntryAssembly()
                        .GetReferencedAssemblies()
                        .Where(a => !a.FullName.StartsWith("System."))
                        .Where(a => !a.FullName.StartsWith("Microsoft."))
                        .Where(a => !a.Name.Equals("System"))
                        .Where(a => !a.FullName.Contains("mscorlib"))
                        .Where(a => !a.FullName.Contains("vshost"))
                        .Where(a => !a.Name.Equals("Microsoft"))
                        .Select(Assembly.Load)
                        .ToList();

            assemblies.Add(_assemblyLoader.GetEntryAssembly());

            return assemblies;
        }
    }
}

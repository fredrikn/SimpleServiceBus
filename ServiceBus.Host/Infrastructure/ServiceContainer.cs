namespace ServiceBus.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ServiceContainer : IServiceContainer
    {
        private readonly IDictionary<Type, IList<object>> _serviceOverrides = new Dictionary<Type, IList<object>>();

        private readonly IDependencyResolver _defaultDependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        /// <param name="defaultDependencyResolver">
        /// The default dependency resolver.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the IDependencyResolver is not specified.
        /// </exception>
        public ServiceContainer(IDependencyResolver defaultDependencyResolver)
        {
            if (defaultDependencyResolver == null)
                throw new ArgumentNullException(nameof(defaultDependencyResolver));

            _defaultDependencyResolver = defaultDependencyResolver;
        }


        /// <summary>
        /// Gets or sets the custom dependency resolver.
        /// </summary>
        public IDependencyResolver CustomDependencyResolver { private get; set; }


        /// <summary>
        /// Determine whether the service type should be fetched with GetService or GetServices.
        /// </summary>
        /// <param name="type">
        /// The type of service to query.
        /// </param>
        /// <returns>
        /// Returns true if the specified service only exists once, else false.
        /// </returns>
        public bool IsSingleService(Type type)
        {
            if (!_serviceOverrides.ContainsKey(type))
                throw new ArgumentException($"Can't find type '{type.Name}'");

            return _serviceOverrides[type].Count == 1;
        }


        /// <summary>
        /// Determine whether the service type should be fetched with GetService or GetServices.
        /// </summary>
        /// <typeparam name="T">
        /// The type of service to query.
        /// </typeparam>
        /// <returns>
        /// Returns true if the specified service only exists once, else false.
        /// </returns>
        public bool IsSingleService<T>()
        {
            return IsSingleService(typeof(T));
        }

        /// <summary>
        /// Adds a service to the end of services list for the given service type.
        /// </summary>
        /// <param name="instance">
        /// The instance of the service to add.
        /// </param>
        /// <typeparam name="T">
        /// The type of the service to add.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// If the instance is not specified.
        /// </exception>
        public void AddService<T>(T instance) where T : class
        {
            AddService(typeof(T), instance);
        }


        /// <summary>
        /// Adds a service to the end of services list for the given service type.
        /// </summary>
        /// <param name="type">
        /// The type of the service to add.
        /// </param>
        /// <param name="instance">
        /// The instance of the service.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If no instance is specified.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the instance is not of the specified type.
        /// </exception>
        public void AddService(Type type, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (!type.IsInstanceOfType(instance))
                throw new ArgumentException($"The given instance of type '{instance.GetType().Name}' is not of correct type in relation to the type {type.FullName}.");

            if (!_serviceOverrides.ContainsKey(type))
                _serviceOverrides[type] = new List<object>();

            _serviceOverrides[type].Add(instance);
        }


        /// <summary>
        /// Gets a service based on the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of a service to retrieve.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }


        /// <summary>
        /// Gets a service based on the specified type.
        /// </summary>
        /// <param name="type">
        /// The type of a service to retrieve.
        /// </param>
        /// <returns>
        /// The service.
        /// </returns>
        /// <exception cref="ResolveDependencyException">
        /// If no type could be find.
        /// </exception>
        public object GetService(Type type)
        {
            if (_serviceOverrides.ContainsKey(type) && !IsSingleService(type))
                throw new ArgumentException("There are more than once instance of this type added, please use the GetServices method.");

            return GetServiceImp(type);
        }

        /// <summary>
        /// Get a list of services for the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the services to get.
        /// </typeparam>
        /// <returns>
        /// A list of services. If no services is found an empty list is returned.
        /// </returns>
        public IEnumerable<T> GetServices<T>()
        {
            return GetServices(typeof(T)).Cast<T>();
        }


        /// <summary>
        /// Get a list of services for the specified type.
        /// </summary>
        /// <param name="type">
        /// The type of the services to get.
        /// </param>
        /// <returns>
        /// A list of services. If no services is found an empty list is returned.
        /// </returns>
        public IEnumerable<object> GetServices(Type type)
        {
            var services = GetOverrideServices(type)
                            ?? GetServicesFromCustomDependencyResolver(type)
                            ?? GetDefaultDependencies(type);

            if (services == null)
                throw new ResolveDependencyException($"Could not resolve dependency of type {type.FullName}");

            return services;
        }

        /// <summary>
        /// Replace one or all instances of the specified type to the specified instance.
        /// </summary>
        /// <param name="instance">
        /// The instance to replace the old instance with.
        /// </param>
        /// <typeparam name="T">
        /// The type of the instance to replace.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// ArgumentNullException is thrown when the specified type is null or if the specified instance is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// ArgumentException will be thrown if the specified type could not be found.
        /// </exception>
        /// <remarks>
        /// The ReplaceService method will only replace services added by using the AddService method. It will not affect or replace 
        /// an underlying custom dependency resolver, or the default dependency resolver.
        /// </remarks>
        public void ReplaceService<T>(T instance) where T : class
        {
            ReplaceService(typeof(T), instance);
        }


        /// <summary>
        /// Replace one or all instances of the specified type to the specified instance.
        /// </summary>
        /// <param name="type">
        /// The type of the instance to replace.
        /// </param>
        /// <param name="instance">
        /// The instance to replace the old instance with.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// ArgumentNullException is thrown when the specified type is null or if the specified instance is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// ArgumentException will be thrown if the specified type could not be found.
        /// </exception>
        /// <remarks>
        /// The ReplaceService method will only replace services added by using the AddService method. It will not affect or replace 
        /// an underlying custom dependency resolver, or the default dependency resolver.
        /// </remarks>
        public void ReplaceService(Type type, object instance)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (!_serviceOverrides.ContainsKey(type))
                throw new ArgumentException($"Can't find a type of '{type.Name}'");

            _serviceOverrides[type] = new List<object> { instance };
        }


        private object GetServiceImp(Type type)
        {
            var service = GetOverrideService(type)
                          ?? GetServiceFromCustomDependencyResolver(type)
                          ?? GetDefaultDependency(type);

            if (service == null)
                throw new ResolveDependencyException($"Could not resolve dependency of type {type.FullName}");

            return service;
        }


        private object GetOverrideService(Type type)
        {
            return _serviceOverrides.ContainsKey(type) ? _serviceOverrides[type].SingleOrDefault() : null;
        }


        private IEnumerable<object> GetOverrideServices(Type type)
        {
            return _serviceOverrides.ContainsKey(type) ? _serviceOverrides[type] : null;
        }


        private object GetServiceFromCustomDependencyResolver(Type type)
        {
            return CustomDependencyResolver?.GetService(type);
        }


        private IEnumerable<object> GetServicesFromCustomDependencyResolver(Type type)
        {
            return CustomDependencyResolver?.GetServices(type);
        }


        private object GetDefaultDependency(Type type)
        {
            return _defaultDependencyResolver.GetService(type);
        }


        private IEnumerable<object> GetDefaultDependencies(Type type)
        {
            return _defaultDependencyResolver.GetServices(type);
        }
    }
}
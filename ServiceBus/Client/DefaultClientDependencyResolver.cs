namespace ServiceBus.Client
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Wcf;

    internal class DefaultClientDependencyResolver : IDependencyResolver
    {
        private readonly IDictionary<Type, object> _cache = new Dictionary<Type, object>();

        public object GetService(Type type)
        {
            if (!_cache.ContainsKey(type))
                _cache[type] = GetServiceInternal(type);

            return _cache[type];
        }


        public IEnumerable<object> GetServices(Type type)
        {
            var service = GetService(type);

            if (service != null)
                return new List<object> { service };

            return new List<object>();
        }


        private static object GetServiceInternal(Type type)
        {
            if (type == typeof(IServiceBusClient))
                return new HttpBasicServiceBusClient();

            return null;
        }
    }
}
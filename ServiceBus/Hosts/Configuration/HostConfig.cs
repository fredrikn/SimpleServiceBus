using ServiceBus.Infrastructure;

namespace ServiceBus.Hosts.Configuration
{
    internal class HostConfig : IHostConfig
    {
        private readonly ServiceContainer _services;

        public HostConfig()
        {
            _services = new ServiceContainer(new DefaultDependencyResolver());
        }

        public IServiceContainer Services => _services;
    }
}

namespace ServiceBus.Client.Configuration
{
    using Infrastructure;

    internal class ClientConfig : IClientConfig
    {
        private readonly ServiceContainer _services;

        public ClientConfig()
        {
            _services = new ServiceContainer(new DefaultClientDependencyResolver());
        }

        public IServiceContainer Services => _services;
    }
}

namespace ServiceBus.Client.TestClient
{
    using Configuration;
    using Wcf.Configuration;


    class Startup : IStartup
    {
        public void Configuration(IClientConfig config)
        {
            //config.UseWcfMsmq();
            config.UseWcfBasicHttp();
        }
    }
}

using System.Diagnostics.Contracts;

namespace ServiceBus.Host.TestHost
{
    using Hosts.Configuration;
    using Hosts.Wcf.Configuration;

    public class Startup : IStartup
    {
        public void Configuration(IHostConfig config)
        {
            //config.UseWcfMsmq();
            config.UseWcfBasicHttp();
        }
    }
}
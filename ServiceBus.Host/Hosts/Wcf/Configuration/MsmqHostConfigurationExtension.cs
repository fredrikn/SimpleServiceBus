namespace ServiceBus.Hosts.Wcf.Configuration
{
    using System;
    using Hosts.Configuration;

    /// <summary>
    /// An extension class to make it easy to configure the Host to use a WCF MSMQ endpoint.
    /// </summary>
    public static class MsmqHostConfigurationExtension
    {
        /// <summary>
        /// An extension method to make it easy to configure the Host to use a WCF MSMQ endpoint.
        /// </summary>
        /// <param name="config">
        /// The IHostConfig.
        /// </param>
        /// <param name="configurationModifier">
        /// A Method for modifying the settings of the MSMQ host.
        /// </param>
        /// <returns>
        /// The <see cref="IHostConfig"/>.
        /// </returns>
        public static IHostConfig UseWcfMsmq(
                                             this IHostConfig config,
                                             Action<IMsmqConfiguration> configurationModifier = null)
        {
            config.Services.AddService(typeof(IHost), CreateAndConfigureMsmqServiceHost(configurationModifier));

            return config;
        }

        private static MsmqServiceHost CreateAndConfigureMsmqServiceHost(
                                              Action<IMsmqConfiguration> configurationModifier)
        {
            var serviceHost = new MsmqServiceHost();

            configurationModifier?.Invoke(serviceHost.Configuration);

            return serviceHost;
        }
    }
}

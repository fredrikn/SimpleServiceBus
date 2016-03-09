namespace ServiceBus.Hosts.Wcf.Configuration
{
    using System;
    using Hosts.Configuration;
 
    
    /// <summary>
    /// An extension class to make it easy to configure the Host to use a WCF Basic HTTP endpoint.
    /// </summary>
    public static class HttpHostConfigurationExtension
    {
        /// <summary>
        /// An extension method to make it easy to configure the Host to use a WCF Basic HTTP endpoint.
        /// </summary>
        /// <param name="config">
        /// The IHostConfig.
        /// </param>
        /// <param name="configurationModifier">
        /// Used is specific configuration settings should be set.
        /// </param>
        /// <returns>
        /// The <see cref="IHostConfig"/>.
        /// </returns>
        public static IHostConfig UseWcfBasicHttp(
                                                  this IHostConfig config,
                                                  Action<IHttpBasicConfiguration> configurationModifier = null)
        {
            config.Services.AddService(typeof(IHost), CreateAndConfigureHttpServiceHost(configurationModifier));

            return config;
        }

        private static HttpServiceHost CreateAndConfigureHttpServiceHost(
                                                Action<IHttpBasicConfiguration> configurationModifier)
        {
            var serviceHost = new HttpServiceHost();

            configurationModifier?.Invoke(serviceHost.Configuration);

            return serviceHost;
        }
    }
}
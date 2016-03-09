namespace ServiceBus.Client.Wcf.Configuration
{
    using System;
    using Client.Configuration;

    /// <summary>
    /// The WCF HTTP client configuration extension. Can be used to configure the ServiceBus to use WCF HTTP for sending commands.
    /// </summary>
    public static class HttpBasicConfigurationExtension
    {
        /// <summary>
        /// Register the use of the WCF HTTP client.
        /// </summary>
        /// <param name="clientConfig">
        /// The client configuration.
        /// </param>
        /// <returns>
        /// The <see cref="IClientConfig"/>.
        /// </returns>
        public static IClientConfig UseWcfBasicHttp(
                                                    this IClientConfig clientConfig,
                                                    Action<IHttpBasicConfiguration> configurationModifier = null)
        {
            clientConfig.Services.AddService<IServiceBusClient>(
                                                CreateAndConfigureHttpBasicClient(configurationModifier));

            return clientConfig;
        }

        private static HttpBasicServiceBusClient CreateAndConfigureHttpBasicClient(
                                                            Action<IHttpBasicConfiguration> configurationModifier)
        {
            var serviceBusClient = new HttpBasicServiceBusClient();

            configurationModifier?.Invoke(serviceBusClient.Configuration);

            return serviceBusClient;
        }
    }
}
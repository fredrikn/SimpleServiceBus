namespace ServiceBus.Client.Wcf.Configuration
{
    using System;
    using Client.Configuration;


    /// <summary>
    /// The WCF MSMQ client configuration extension. Can be used to configure the ServiceBus to use WCF MSMQ for sending commands.
    /// </summary>
    public static class MsmqClientConfigurationExtension
    {
        /// <summary>
        /// Register the use of the WCF MSMQ client.
        /// </summary>
        /// <param name="clientConfig">
        /// The client configuration.
        /// </param>
        /// <param name="configurationModifier">
        /// The configuration Modifier.
        /// </param>
        /// <returns>
        /// The <see cref="IClientConfig"/>.
        /// </returns>
        public static IClientConfig UseWcfMsmq(
                                               this IClientConfig clientConfig,
                                               Action<IMsmqConfiguration> configurationModifier = null)
        {
            clientConfig.Services.AddService<IServiceBusClient>(
                                                CreateAndConfigureMsmqClient(configurationModifier));

            return clientConfig;
        }

        private static MsmqServiceBusClient CreateAndConfigureMsmqClient(
                                                    Action<IMsmqConfiguration> configurationModifier)
        {
            var serviceBusClient = new MsmqServiceBusClient();

            configurationModifier?.Invoke(serviceBusClient.Configuration);

            return serviceBusClient;
        }
    }
}
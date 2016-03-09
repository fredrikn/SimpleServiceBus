namespace ServiceBus.Client.Configuration
{
    /// <summary>
    /// An interface for defining a class which configuration will be applied during startup.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Configures the specified client configuration.
        /// </summary>
        /// <param name="config">The client configuration.</param>
        void Configuration(IClientConfig config);
    }
}

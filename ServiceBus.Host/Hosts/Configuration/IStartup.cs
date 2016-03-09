namespace ServiceBus.Hosts.Configuration
{
    /// <summary>
    /// An interface for defining a class which configuration will be applied during startup.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Configures the specified host configuration.
        /// </summary>
        /// <param name="config">The host configuration.</param>
        void Configuration(IHostConfig config);
    }
}

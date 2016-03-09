using ServiceBus.Infrastructure;

namespace ServiceBus.Hosts.Configuration
{
    /// <summary>
    /// The configuration for the CServiceBusHost.
    /// </summary>
    public interface IHostConfig
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        IServiceContainer Services { get; }
    }
}

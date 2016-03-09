namespace ServiceBus.Client.Configuration
{
    using Infrastructure;

    /// <summary>
    /// The ClientConfiguration interface.
    /// </summary>
    public interface IClientConfig
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        IServiceContainer Services { get; }
    }
}
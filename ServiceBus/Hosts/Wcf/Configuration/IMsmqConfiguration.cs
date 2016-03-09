namespace ServiceBus.Hosts.Wcf.Configuration
{
    using System;

    /// <summary>
    /// The class used to get Wcf MSMQ Configurations.
    /// </summary>
    public interface IMsmqConfiguration
    {

        /// <summary>
        /// Gets or sets the maximum retry cycles.
        /// </summary>
        int MaxRetryCycles { get; set; }

        /// <summary>
        /// Gets or sets the receive retry count.
        /// </summary>
        int ReceiveRetryCount { get; set; }

        /// <summary>
        /// Gets the retry cycle delay.
        /// </summary>
        TimeSpan RetryCycleDelay { get; set; }

        /// <summary>
        /// Gets the retry cycle delay.
        /// </summary>
        string QueueName { get; set; }
    }
}
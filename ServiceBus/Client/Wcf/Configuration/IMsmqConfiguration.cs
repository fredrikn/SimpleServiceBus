namespace ServiceBus.Client.Wcf.Configuration
{
    using System;
    using Contracts;

    /// <summary>
    /// The class used to get Wcf MSMQ Configurations.
    /// </summary>
    public interface IMsmqConfiguration
    {
        /// <summary>
        /// Gets the MSMQ server.
        /// </summary>
        string MsmqServer { get; set; }

        /// <summary>
        /// Gets the receive retry count.
        /// </summary>
        int ReceiveRetryCount { get; set; }

        /// <summary>
        /// Gets the max retry cycles.
        /// </summary>
        int MaxRetryCycles { get; set; }

        /// <summary>
        /// Gets the retry cycle delay.
        /// </summary>
        TimeSpan RetryCycleDelay { get; set; }

        /// <summary>
        /// Gets the time before a message will be expired and moved to a dead-letter queue.
        /// </summary>
        TimeSpan TimeToLive { get; set; }

        /// <summary>
        /// Get the end point address queue for this command. Throws if undefined.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The end point address.
        /// </returns>
        string GetEndpointAddressForCommand(Command command);

        /// <summary>
        /// Get the dead letter queue for command. Null if no DLQ is defined.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The dead letter queue.
        /// </returns>
        string GetDeadLetterQueueForCommand(Command command);
    }
}
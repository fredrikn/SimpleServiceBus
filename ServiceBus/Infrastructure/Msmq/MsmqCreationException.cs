namespace ServiceBus.Infrastructure.Msmq
{
    using System;

    /// <summary>
    /// En exception thrown when something goes wrong during the creation of a messaging queue.
    /// </summary>
    [Serializable]
    public class MessageQueueCreationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueCreationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The inner eception that may hold additional info about the excpetion that cause this exception to be thrown.</param>
        public MessageQueueCreationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
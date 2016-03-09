namespace ServiceBus.Hosts
{
    using System;

    /// <summary>
    /// Throws when a Host can't be started.
    /// </summary>
    [Serializable]
    public class HostStartupException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostStartupException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HostStartupException(string message) : base(message)
        {
        }
    }
}

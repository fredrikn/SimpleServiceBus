namespace ServiceBus.Infrastructure
{
    using System;

    /// <summary>
    /// Will be thrown when a Service can't be resolved.
    /// </summary>
    [Serializable]
    public class ResolveDependencyException : Exception
    {
        public ResolveDependencyException(string message)
            : base(message)
        {
        }


        public ResolveDependencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

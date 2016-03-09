namespace ServiceBus.Infrastructure
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An interface for encapsulating inversion of control.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Gets an instance of a service. Should NEVER throw an exception.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An object of the inputted type.</returns>
        object GetService(Type type);


        /// <summary>
        /// Gets a list of instances of services. Should NEVER throw an exception.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of services mapped to the specified type.</returns>
        IEnumerable<object> GetServices(Type type);
    }
}
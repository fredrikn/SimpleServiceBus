namespace ServiceBus.Infrastructure
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Container for registreing Services used by the LServiceBusHost.
    /// </summary>
    public interface IServiceContainer
    {
        /// <summary>
        /// Adds a service to the end of services list for the given service type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the service to add.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a service to add.
        /// </param>
        void AddService<T>(T instance) where T : class;


        /// <summary>
        /// Adds a service to the end of services list for the given service type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the service to add.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a service to add.
        /// </param>
        void AddService(Type type, object instance);


        /// <summary>
        /// Replace one or all instances of the specified type to the specified instance.
        /// </summary>
        /// <param name="instance">
        /// The instance to replace the old instance with.
        /// </param>
        /// <typeparam name="T">
        /// The type of the instance to replace.
        /// </typeparam>
        /// <remarks>
        /// The ReplaceService method will only replace services added by using the AddService method. It will not affect or replace 
        /// an underlaying custom dependency resolver, or the default dependency resolver.
        /// </remarks>
        void ReplaceService<T>(T instance) where T : class;


        /// <summary>
        /// Replace one or all instances of the specified type to the specified instance.
        /// </summary>
        /// <param name="type">
        /// The type of the instance to replace.
        /// </param>
        /// <param name="instance">
        /// The instance to replace the old instance with.
        /// </param>
        /// <remarks>
        /// The ReplaceService method will only replace services added by using the AddService method. It will not affect or replace 
        /// an underlaying custom dependency resolver, or the default dependency resolver.
        /// </remarks>
        void ReplaceService(Type type, object instance);


        /// <summary>
        /// Gets a instance of the specified service type.
        /// </summary>
        /// <param name="type">The type of the service to get an instance of.</param>
        /// <returns>Returns an instance of the specified type.</returns>
        object GetService(Type type);


        /// <summary>
        /// Gets a list of instances of the specified generic type.
        /// </summary>
        /// <param name="T">The type of the service to get an instance of.</param>
        /// <returns>Returns a list of instances of the specified type.</returns>
        IEnumerable<T> GetServices<T>();


        /// <summary>
        /// Gets a list of instances of the specified type.
        /// </summary>
        /// <param name="type">The type of the service to get an instance of.</param>
        /// <returns>Returns a list of instances of the specified type.</returns>
        IEnumerable<object> GetServices(Type type);


        /// <summary>
        /// Gets a instance of the specified service type.
        /// </summary>
        /// <param name="T">The type of the service to get an instance of.</param>
        /// <returns>Returns an instance of the specified type.</returns>
        T GetService<T>();


        /// <summary>
        /// Determine whether the service type should be fetched with GetService or GetServices.
        /// </summary>
        /// <param name="serviceType">
        /// The type of service to query.
        /// </param>
        /// <returns>
        /// Returns true if the specified service only exists once, else false.
        /// </returns>
        /// <remarks>
        /// The ISingleService method will only work with services added by using the AddService method. It will query  
        /// an underlaying custom dependency resolver, or the default dependency resolver for that instance.
        /// </remarks>
        bool IsSingleService(Type serviceType);


        /// <summary>
        /// Determine whether the service type should be fetched with GetService or GetServices.
        /// </summary>
        /// <typeparam name="T">
        /// The type of service to query.
        /// </typeparam>
        /// <returns>
        /// Returns true if the specified service only exists once, else false.
        /// </returns>
        /// <remarks>
        /// The ISingleService method will only work with services added by using the AddService method. It will query  
        /// an underlaying custom dependency resolver, or the default dependency resolver for that instance.
        /// </remarks>
        bool IsSingleService<T>();


        /// <summary>
        /// Returns the Dependency Resolver for the Services. 
        /// </summary>
        IDependencyResolver CustomDependencyResolver { set; }
    }
}
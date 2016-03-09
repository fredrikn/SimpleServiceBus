namespace ServiceBus.Client
{
    using System;
    using Contracts;

    /// <summary>
    /// ServiceBus client. Responsible to send a Command or an Event to a ServiceBus Host.
    /// </summary>
    public interface IServiceBusClient : IDisposable
    {
        /// <summary>
        /// Send a command to a ServiceBus host to be handled.
        /// </summary>
        /// <param name="command">
        /// The command to be handled.
        /// </param>
        void SendCommand(Command command);

        /// <summary>
        /// Send a Command to a host to be handled.
        /// </summary>
        /// <param name="command">The command to be handled.</param>
        /// <param name="endpointAddress">
        /// The Servicebus endpoint address. The addres format may be different based on the 
        /// Client used for the ServiceBus.
        /// </param>
        /// <remarks>
        /// If the Client of the ServiceBus supports an endpoint address for where the Command
        /// should be sent, the enpointAddress can be specified.
        /// </remarks>
        void SendCommand(Command command, string endpointAddress);
    }
}
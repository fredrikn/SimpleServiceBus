namespace ServiceBus.Hosts.CommandService
{
    using Contracts;

    /// <summary>
    /// Represents the interface for handing the execution of a command.
    /// </summary>
    public interface ICommandHandlerInvoker
    {
        /// <summary>
        /// Invoking the specified CommandHandler
        /// </summary>
        /// <param name="command">The Command to be passed as an argument to the commandHandler</param>
        /// <param name="commandHandler">The Command handler that should be invoked</param>
        void Invoke(Command command, dynamic commandHandler);
    }
}
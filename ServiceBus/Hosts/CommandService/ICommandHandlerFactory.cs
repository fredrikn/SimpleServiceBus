namespace ServiceBus.Hosts.CommandService
{
    using Contracts;

    /// <summary>
    /// A factory for creating ICommandHandlers.
    /// </summary>
    public interface ICommandHandlerFactory
    {
        /// <summary>
        /// Creates a handler for the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A command handler.</returns>
        object Create(Command command);
    }
}
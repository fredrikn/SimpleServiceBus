namespace ServiceBus.Hosts.CommandService
{
    using Contracts;

    /// <summary>
    /// The Command handler that handle a specific Commad.
    /// </summary>
    /// <typeparam name="T">
    /// The Type of a Command.
    /// </typeparam>
    public interface ICommandHandler<in T> where T : Command
    {
        /// <summary>
        /// The method to handle a command.
        /// </summary>
        /// <param name="command">
        /// The command to handle.
        /// </param>
        void Handle(T command);
    }
}
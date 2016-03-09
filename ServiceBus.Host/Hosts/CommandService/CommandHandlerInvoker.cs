namespace ServiceBus.Hosts.CommandService
{
    using System;
    using Contracts;

    /// <summary>
    /// Invokes the Command handler with a specified Command.
    /// </summary>
    public class CommandHandlerInvoker : ICommandHandlerInvoker
    {
        /// <summary>
        /// Invokes the Command handler with the specified Command.
        /// </summary>
        /// <param name="command">The Command to send to the CommandHandler.</param>
        /// <param name="commandHandler">The Command Handler for the specified Command.</param>
        public virtual void Invoke(Command command, dynamic commandHandler)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (commandHandler == null)
                throw new ArgumentNullException(nameof(commandHandler));

            try
            {
                commandHandler.Handle((dynamic)command);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    $"Failed while handling command '{command.GetType().FullName}' with CommandHandler '{commandHandler.GetType().FullName}', see inner exception for more details.",
                    e);
            }
        }
    }
}
namespace ServiceBus.Hosts.CommandService
{
    using Contracts;
    using Infrastructure;

    /// <summary>
    /// A factory for creating ICommandHandlers.
    /// </summary>
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceContainer _serviceContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerFactory"/> class.
        /// </summary>
        public CommandHandlerFactory()
        {
            _serviceContainer = ServiceBusHost.Config.Services;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerFactory"/> class.
        /// </summary>
        /// <param name="serviceContainer">The service container.</param>
        public CommandHandlerFactory(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        /// <summary>
        /// Creates a handler for the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A command handler.</returns>
        public virtual object Create(Command command)
        {
            var type = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            return _serviceContainer.GetService(type);
        }
    }
}
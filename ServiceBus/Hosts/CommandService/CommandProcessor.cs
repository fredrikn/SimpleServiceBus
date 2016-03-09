namespace ServiceBus.Hosts.CommandService
{
    using System;
    using Contracts;
    using Infrastructure.Validation;

    /// <summary>
    /// The service.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandHandlerFactory _handlerFactory;
        private readonly ICommandHandlerInvoker _handlerInvoker;
        private readonly ICommandProcessorInspector _commandProcessorInspector;
        private readonly ICommandValidator _commandValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        public CommandProcessor()
        {
            _handlerFactory = ServiceBusHost.Config.Services.GetService<ICommandHandlerFactory>();
            _handlerInvoker = ServiceBusHost.Config.Services.GetService<ICommandHandlerInvoker>();
            _commandProcessorInspector = ServiceBusHost.Config.Services.GetService<ICommandProcessorInspector>();
            _commandValidator = ServiceBusHost.Config.Services.GetService<ICommandValidator>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        /// <param name="handlerFactory">
        /// The handler factory.
        /// </param>
        /// <param name="handlerInvoker">
        /// The handler invoker.
        /// </param>
        /// <param name="commandProcessorInspector">
        /// A Command processor inspector, can be used to inspect Command before and after it's invoked.
        /// </param>
        /// <param name="commandValidator">
        /// The validator to validate the command.
        /// </param>
        public CommandProcessor(
                                ICommandHandlerFactory handlerFactory,
                                ICommandHandlerInvoker handlerInvoker,
                                ICommandProcessorInspector commandProcessorInspector,
                                ICommandValidator commandValidator)
        {
            if (handlerFactory == null)
                throw new ArgumentNullException(nameof(handlerFactory));

            if (handlerInvoker == null)
                throw new ArgumentNullException(nameof(handlerInvoker));

            if (commandProcessorInspector == null)
                throw new ArgumentNullException(nameof(commandProcessorInspector));

            if (commandValidator == null)
                throw new ArgumentNullException(nameof(commandValidator));

            _handlerFactory = handlerFactory;
            _handlerInvoker = handlerInvoker;
            _commandProcessorInspector = commandProcessorInspector;
            _commandValidator = commandValidator;
        }

        /// <summary>
        /// Will handle the command and execute its CommandHandler.
        /// </summary>
        /// <param name="command">
        /// The command to handle.
        /// </param>
        public virtual void Handle(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            try
            {
                ProcessCommand(command);
            }
            catch (Exception e)
            {
                _commandProcessorInspector.FailedProcessing(command, e);
                throw;
            }
        }


        private void ProcessCommand(Command command)
        {
            _commandProcessorInspector.BeforeProcessing(command);

            _commandValidator.Validate(command, ValidationPath.Deep);

            HandleImp(command);

            _commandProcessorInspector.AfterProcessing(command);
        }


        private void HandleImp(Command command)
        {
            var handler = _handlerFactory.Create(command);

            _handlerInvoker.Invoke(command, handler);
        }
    }
}
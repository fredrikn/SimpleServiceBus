namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.Transactions;
    using Contracts;
    using CommandService;

    /// <summary>
    /// The command subscriber receiver that will get a command and execute the correct command handlers for it.
    /// </summary>
    [ServiceBehavior(
                     ConcurrencyMode = ConcurrencyMode.Single,
                     AddressFilterMode = AddressFilterMode.Any,
                     TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                     IncludeExceptionDetailInFaults = true)]
    public class CommandReceiver : ICommandReceiver
    {
        private readonly ICommandProcessor _processor;


        /// <summary>
        /// Initializes a new instance of the <see cref="CommandReceiver"/> class.
        /// </summary>
        public CommandReceiver()
        {
            _processor = ServiceBusHost.Config.Services.GetService<ICommandProcessor>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CommandReceiver"/> class.
        /// </summary>
        /// <param name="processor">
        /// The command service.
        /// </param>
        public CommandReceiver(ICommandProcessor processor)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            _processor = processor;
        }


        /// <summary>
        /// Receives the specified command.
        /// </summary>
        /// <param name="command">The command to be processed.</param>
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void ReceiveCommand(Command command)
        {
            Trace.WriteLine($"{DateTime.Now} - Received command '{command.GetType().FullName}'");
            _processor.Handle(command);
        }
    }
}
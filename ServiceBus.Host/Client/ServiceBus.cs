namespace ServiceBus.Client
{
    using System;
    using Configuration;
    using Contracts;
    using Infrastructure.Validation;
    using Wcf;
    
    
    /// <summary>
    /// The ServiceBus client.
    /// </summary>
    public sealed class ServiceBus : IServiceBus, IDisposable
    {
        private readonly IServiceBusClient _client;
        private readonly ICommandValidator _annotationValidator;


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBus"/> class.
        /// </summary>
        /// <remarks>
        /// Will use WCF MSQM Service Bus Client as default.
        /// </remarks>
        public ServiceBus()
            : this(new MsmqServiceBusClient(), new AnnotationCommandValidator())
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBus"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/> will be thrown is no client is specified.
        /// </exception>
        public ServiceBus(IServiceBusClient client) : this(client, new AnnotationCommandValidator())
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBus"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="annotationValidator">
        /// Specifies the IAnnotationValidator that should be used to validate if a Command is valid.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/> will be thrown if no client and/or annotationValidator is specified.
        /// </exception>
        public ServiceBus(IServiceBusClient client, ICommandValidator annotationValidator)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (annotationValidator == null)
                throw new ArgumentNullException(nameof(annotationValidator));

            _client = client;
            _annotationValidator = annotationValidator;
        }


        /// <summary>
        /// Creates a ServiceBus.
        /// </summary>
        /// <typeparam name="TStartUp">
        /// A type of <see cref="IStartup"/> that will be used to configure the client.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ServiceBus"/>.
        /// </returns>
        public static ServiceBus Create<TStartUp>() where TStartUp : IStartup, new()
        {
            var clientConfig = new ClientConfig();

            var configurationManager = new ConfigurationManager();
            configurationManager.Config<TStartUp>(clientConfig);

            return new ServiceBus(clientConfig.Services.GetService<IServiceBusClient>());
        }


        /// <summary>
        /// Send a Command to a host to be handled.
        /// </summary>
        /// <param name="command">The command to be handled.</param>
        public void Send(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _annotationValidator.Validate(command, ValidationPath.Deep);

            _client.SendCommand(command);
        }


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
        public void Send(Command command, string endpointAddress)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _annotationValidator.Validate(command, ValidationPath.Deep);

            _client.SendCommand(command, endpointAddress);
        }


        /// <summary>
        /// Dispose the ServiceBus.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
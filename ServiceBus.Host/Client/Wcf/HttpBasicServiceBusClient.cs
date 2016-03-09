namespace ServiceBus.Client.Wcf
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Configuration;
    using Contracts;
    using Hosts.Wcf;

    /// <summary>
    /// A WCF basic HTTP service bus client.
    /// </summary>
    public sealed class HttpBasicServiceBusClient : IServiceBusClient
    {
        private readonly IChannelFactory<ICommandReceiver> _channelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicServiceBusClient"/> class.
        /// </summary>
        public HttpBasicServiceBusClient()
        {
            var myBinding = new BasicHttpBinding();
            _channelFactory = new ChannelFactory<ICommandReceiver>(myBinding);
            Configuration = new HttpBasicConfiguration();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicServiceBusClient"/> class.
        /// </summary>
        public HttpBasicServiceBusClient(IHttpBasicConfiguration httpBasicConfiguration)
        {
            if (httpBasicConfiguration == null)
                throw new ArgumentNullException(nameof(httpBasicConfiguration));

            var myBinding = new BasicHttpBinding();
            _channelFactory = new ChannelFactory<ICommandReceiver>(myBinding);
            Configuration = httpBasicConfiguration;
        }



        /// <summary>
        /// The Configuration of the HttpBasicServiceBusClient.
        /// </summary>
        public IHttpBasicConfiguration Configuration { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicServiceBusClient"/> class.
        /// </summary>
        /// <param name="channelFactory">
        /// The channel factory to be used to create a channel for the service communication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The ArgumentNullException will be thrown if not channel factory is specified.
        /// </exception>
        public HttpBasicServiceBusClient(IChannelFactory<ICommandReceiver> channelFactory)
        {
            if (channelFactory == null)
                throw new ArgumentNullException(nameof(channelFactory));

            _channelFactory = channelFactory;
        }


        /// <summary>
        /// The Send method will send the specified command to the host for handling.
        /// </summary>
        /// <param name="command">
        /// The command to be handled.
        /// </param>
        public void SendCommand(Command command)
        {
            SendCommandImp(command, Configuration.Url);
        }


        /// <summary>
        /// The Send method will send the specified command to the host for handling.
        /// </summary>
        /// <param name="command">
        /// The command to be handled.
        /// </param>
        /// <param name="endpointAddress">
        /// Specifies the endpoint address of the Wcf Service.
        /// </param>
        public void SendCommand(Command command, string endpointAddress)
        {
            if (string.IsNullOrWhiteSpace(endpointAddress))
                throw new ArgumentNullException(nameof(endpointAddress));

            SendCommandImp(command, new Uri(endpointAddress));
        }


        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            _channelFactory?.Close();
        }

        private void SendCommandImp(Command command, Uri endpointAddress)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            try
            {
                var client = CreateClient(endpointAddress);
                client.ReceiveCommand(command);
            }
            catch (Exception e)
            {
                throw new CommunicationException("Can't communicate with the remote service, check inner exception for more details.", e);
            }
        }


        private ICommandReceiver CreateClient(Uri endpointAddress)
        {
            var client = _channelFactory.CreateChannel(new EndpointAddress((endpointAddress)));
            return client;
        }
    }
}
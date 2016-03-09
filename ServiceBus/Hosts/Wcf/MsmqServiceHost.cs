namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.ServiceModel;
    using Configuration;
    using Infrastructure.Msmq;

    /// <summary>
    /// A service host using a MSMQ binding (WCF).
    /// </summary>
    public sealed class MsmqServiceHost : IHost, IDisposable
    {
        private const int FOUR_MEGABYTES_IN_BYTES = 4194304;
        private readonly MsmqSetup _msmqSetup;
        private readonly WcfServiceHost _serviceHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceHost"/> class.
        /// </summary>
        public MsmqServiceHost()
        {
            _serviceHost = new WcfServiceHost(typeof(CommandReceiver));
            _msmqSetup = new MsmqSetup();
            Configuration = ServiceBusHost.Config.Services.GetService<IMsmqConfiguration>();
        }


        /// <summary>
        /// Gets or sets the configuration settings.
        /// </summary>
        public IMsmqConfiguration Configuration { get; }


        /// <summary>
        /// Opens this service host.
        /// </summary>
        public void Open()
        {
            _msmqSetup.EnsureQueueExists(Configuration.QueueName);

            var msmqAddress = $"net.msmq://localhost/private/{Configuration.QueueName}";

            _serviceHost.AddServiceEndpoint(
                                            typeof(ICommandReceiver),
                                            CreateNetMsmqBinding(),
                                            msmqAddress);

            _serviceHost.Open();

            if (Environment.UserInteractive)
                Console.WriteLine("The service is ready at {0}", msmqAddress);
        }

        /// <summary>
        /// Closes this service host.
        /// </summary>
        public void Close()
        {
            _serviceHost?.Close();
        }


        public void Dispose()
        {
            _serviceHost?.Close();
        }



        /// <summary>
        /// Creates the net MSMQ binding.
        /// </summary>
        /// <returns>A net MSMQ binding.</returns>
        private NetMsmqBinding CreateNetMsmqBinding()
        {
            return new NetMsmqBinding
            {
                RetryCycleDelay = Configuration.RetryCycleDelay,
                MaxRetryCycles = Configuration.MaxRetryCycles,
                ReceiveRetryCount = Configuration.ReceiveRetryCount,
                ReceiveErrorHandling = ReceiveErrorHandling.Move,
                MaxReceivedMessageSize = FOUR_MEGABYTES_IN_BYTES,
                Security = new NetMsmqSecurity
                {
                    Mode = NetMsmqSecurityMode.None
                },
            };
        }
    }
}
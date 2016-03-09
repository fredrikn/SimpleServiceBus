namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.ServiceModel;
    using Configuration;

    /// <summary>
    /// A service host using a basic HTTP binding (WCF).
    /// </summary>
    public sealed class HttpServiceHost : IHost, IDisposable
    {
        private readonly ServiceHost _serviceHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceHost"/> class.
        /// </summary>
        public HttpServiceHost()
        {
            _serviceHost = new WcfServiceHost(typeof(CommandReceiver));
            Configuration = ServiceBusHost.Config.Services.GetService<IHttpBasicConfiguration>();
        }


        /// <summary>
        /// Gets or sets the configuration settings.
        /// </summary>
        public IHttpBasicConfiguration Configuration { get; }


        /// <summary>
        /// Opens this service host.
        /// </summary>
        public void Open()
        {
            _serviceHost.AddServiceEndpoint(
                                            typeof(ICommandReceiver),
                                            new BasicHttpBinding(),
                                            Configuration.Url);

            _serviceHost.Open();

            if (Environment.UserInteractive)
                Console.WriteLine($"The service is ready at {Configuration.Url}");
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
    }
}
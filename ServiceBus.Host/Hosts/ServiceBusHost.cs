namespace ServiceBus.Hosts
{
    using System;
    using Configuration;


    public sealed class ServiceBusHost : IDisposable
    {
        private readonly IHost _host;

        private ServiceBusHost()
        {
            _host = Config.Services.GetService<IHost>();
        }

        private static ServiceBusHost Current { get; set; }


        /// <summary>
        /// Gets the host configuration.
        /// </summary>
        public static IHostConfig Config { get; private set; }


        /// <summary>
        /// Starts a new host.
        /// </summary>
        /// <typeparam name="TStartUp">The type of the start up class.</typeparam>
        /// <returns>The started host.</returns>
        /// <exception cref="HostStartupException">There is already a host running.</exception>
        public static ServiceBusHost Start<TStartUp>() where TStartUp : IStartup, new()
        {
            if (Current != null)
                throw new HostStartupException("There is already a host running.");

            Config = new HostConfig();
            new TStartUp().Configuration(Config);

            Current = new ServiceBusHost();

            Current.Start();

            return Current;
        }

        /// <summary>
        /// Dispose the ServiceBusHost
        /// </summary>
        public void Dispose()
        {
            Stop();
            Current = null;
        }

        /// <summary>
        /// Stops the host.
        /// </summary>
        public void Stop()
        {
            _host.Close();
        }

        private void Start()
        {
            _host.Open();
        }
    }
}
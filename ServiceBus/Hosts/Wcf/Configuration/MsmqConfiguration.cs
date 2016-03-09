namespace ServiceBus.Hosts.Wcf.Configuration
{
    using System;
    using Infrastructure.Configuration;
    using System.Text.RegularExpressions;
    using Infrastructure;


    /// <summary>
    /// The WCF MSMQ configuration.
    /// </summary>
    public class MsmqConfiguration : IMsmqConfiguration
    {
        private int? _receiveRetryCount = null;
        private int? _maxRetryCycles = null;
        private TimeSpan? _retryCycleDelay = null;
        private string _queueName;

        private readonly IConfigurationReader _configurationReader;
        private readonly IAssemblyLoader _assemblyLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqConfiguration" /> class.
        /// </summary>
        public MsmqConfiguration()
            : this(
            ServiceBusHost.Config.Services.GetService<IConfigurationReader>(),
            new AssemblyLoader())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqConfiguration" /> class.
        /// </summary>
        /// <param name="configurationReader">The IConfigurationReader to get configuration from.</param>
        /// <param name="assemblyLoader">The assembly loader.</param>
        public MsmqConfiguration(
            IConfigurationReader configurationReader,
            IAssemblyLoader assemblyLoader)
        {
            if (configurationReader == null)
                throw new ArgumentNullException(nameof(configurationReader));

            if (assemblyLoader == null)
                throw new ArgumentNullException(nameof(assemblyLoader));

            _configurationReader = configurationReader;
            _assemblyLoader = assemblyLoader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqConfiguration"/> class.
        /// </summary>
        /// <param name="configReader">
        /// The IConfigurationReader to get configuration from.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The ArgumentNullException will be thrown if no configReader is specified.
        /// </exception>
        public MsmqConfiguration(IConfigurationReader configReader)
        {
            if (configReader == null)
                throw new ArgumentNullException(nameof(configReader));

            _configurationReader = configReader;
        }


        /// <summary>
        /// Gets or sets the receive retry count.
        /// </summary>
        /// <remarks>
        /// The default value is '5', to configure the value add a the key 'ServiceBus.MsmqHost.ReceiveRetryCount' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public int ReceiveRetryCount
        {
            get
            {
                if (_receiveRetryCount.HasValue)
                    return _receiveRetryCount.Value;

                _receiveRetryCount = _configurationReader.GetNumericValue("ServiceBus.MsmqHost.ReceiveRetryCount") ?? 5;
                return _receiveRetryCount.Value;
            }
            set { _receiveRetryCount = value; }
        }


        /// <summary>
        /// Gets the max retry cycles.
        /// </summary>
        /// <remarks>
        /// The default value is '10', to configure the value add a the key 'ServiceBus.MsmqHost.MaxRetryCycles' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public int MaxRetryCycles
        {
            get
            {
                if (_maxRetryCycles.HasValue)
                    return _maxRetryCycles.Value;

                _maxRetryCycles = _configurationReader.GetNumericValue("ServiceBus.MsmqHost.MaxRetryCycles") ?? 10;

                return _maxRetryCycles.Value;
            }
            set { _maxRetryCycles = value; }
        }


        /// <summary>
        /// Gets the retry cycle delay.
        /// </summary>
        /// <remarks>
        /// The default value is 10 minutes, to configure the value add a the key 'ServiceBus.MsmqHost.RetryCycleDelay' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public TimeSpan RetryCycleDelay
        {
            get
            {
                if (_retryCycleDelay.HasValue)
                    return _retryCycleDelay.Value;

                _retryCycleDelay = _configurationReader.GetTimeSpanValue("ServiceBus.MsmqHost.RetryCycleDelay") ?? new TimeSpan(0, 10, 0);

                return _retryCycleDelay.Value;
            }
            set { _retryCycleDelay = value; }
        }

        /// <summary>
        /// Gets or sets the Queue name to retrieve the command from.
        /// </summary>
        /// <remarks>
        /// The default value is the name of the entry assembly, to configure the value add a the key 'ServiceBus.MsmqHost.QueueName' to the configuration source and give it a value.
        /// </remarks>
        public string QueueName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_queueName))
                    return _queueName;

                var stringValue = _configurationReader.GetStringValue("ServiceBus.MsmqHost.QueueName");

                var queueName = stringValue ?? _assemblyLoader.GetEntryAssembly().GetName().Name;

                _queueName = Regex.Replace(queueName, @"[\s]*", string.Empty);

                return _queueName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(QueueName));

                _queueName = value;
            }
        }
    }
}
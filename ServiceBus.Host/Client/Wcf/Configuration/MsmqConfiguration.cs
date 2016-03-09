

namespace ServiceBus.Client.Wcf.Configuration
{
    using System;
    using System.Configuration;
    using Contracts;
    using Infrastructure.Configuration;

    /// <summary>
    /// The WCF MSMQ configuration.
    /// </summary>
    public class MsmqConfiguration : IMsmqConfiguration
    {
        private readonly IConfigurationReader _configurationReader;

        private string _msmqServer;
        private int? _receiveRetryCount = null;
        private int? _maxRetryCycles = null;
        private TimeSpan? _retryCycleDelay = null;
        private TimeSpan? _timeToLive = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqConfiguration"/> class.
        /// </summary>
        public MsmqConfiguration()
        {
            _configurationReader = new AppConfigReader();
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
        /// Gets or sets the MSMQ server to deliver the command to.
        /// </summary>
        /// <remarks>
        /// The default value is 'localhost', to configure the value add a the key 'ServiceBus.MsmqClient.Server' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public string MsmqServer
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_msmqServer))
                    return _msmqServer;

                var value = _configurationReader.GetStringValue("ServiceBus.MsmqClient.Server");
                _msmqServer = !string.IsNullOrWhiteSpace(value) ? value : "localhost";

                return _msmqServer;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(MsmqServer));

                _msmqServer = value;
            }
        }

        /// <summary>
        /// Gets or sets the receive retry count.
        /// </summary>
        /// <remarks>
        /// The default value is '5', to configure the value add a the key 'ServiceBus.MsmqClient.ReceiveRetryCount' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public int ReceiveRetryCount
        {
            get
            {
                if (_receiveRetryCount.HasValue)
                    return _receiveRetryCount.Value;

                _receiveRetryCount = _configurationReader.GetNumericValue("ServiceBus.MsmqClient.ReceiveRetryCount") ?? 5;
                return _receiveRetryCount.Value;
            }
            set { _receiveRetryCount = value; }
        }


        /// <summary>
        /// Gets the max retry cycles.
        /// </summary>
        /// <remarks>
        /// The default value is '10', to configure the value add a the key 'ServiceBus.MsmqClient.MaxRetryCycles' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public int MaxRetryCycles
        {
            get
            {
                if (_maxRetryCycles.HasValue)
                    return _maxRetryCycles.Value;

                _maxRetryCycles = _configurationReader.GetNumericValue("ServiceBus.MsmqClient.MaxRetryCycles") ?? 10;

                return _maxRetryCycles.Value;
            }
            set { _maxRetryCycles = value; }
        }


        /// <summary>
        /// Gets the retry cycle delay.
        /// </summary>
        /// <remarks>
        /// The default value is 10 minutes, to configure the value add a the key 'ServiceBus.MsmqClient.RetryCycleDelay' to the configuration source and give it a value.
        /// or set the value explicitly.
        /// </remarks>
        public TimeSpan RetryCycleDelay
        {
            get
            {
                if (_retryCycleDelay.HasValue)
                    return _retryCycleDelay.Value;

                _retryCycleDelay = _configurationReader.GetTimeSpanValue("ServiceBus.MsmqClient.RetryCycleDelay") ?? new TimeSpan(0, 10, 0);

                return _retryCycleDelay.Value;
            }
            set { _retryCycleDelay = value; }
        }


        /// <summary>
        /// Gets the time a message will live in a queue before it will be expired and moved to a dead-letter queue.
        /// The default is 1 day.
        /// </summary>
        /// <remarks>
        /// To configure the value add a the key 'ServiceBus.MsmqClient.TimeToLive' to the configuration source and give it a value.
        /// </remarks>
        public TimeSpan TimeToLive
        {
            get
            {
                if (_timeToLive.HasValue)
                    return _timeToLive.Value;

                _timeToLive = _configurationReader.GetTimeSpanValue("ServiceBus.MsmqClient.TimeToLive") ??
                              new TimeSpan(1, 0, 0);

                return _timeToLive.Value;
            }
            set { _timeToLive = value; }
        }


        /// <summary>
        /// Get the end point address queue for this command. Throws if undefined.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <remarks>
        /// To configure the value add a the key 'ServiceBus.MsmqClient.EndpointAddress.{CommandType}' to the configuration source and give it a value.
        /// </remarks>
        /// <returns>
        /// The end point address.
        /// </returns>
        public string GetEndpointAddressForCommand(Command command)
        {
            return GetValueForCommandType(
                command: command,
                keyFormat: "ServiceBus.MsmqClient.EndpointAddress.{0}",
                allowMissingConfig: false);
        }

        /// <summary>
        /// Get the dead letter queue for command. Null if no DLQ is defined.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <remarks>
        /// To configure the value add a the key 'ServiceBus.MsmqClient.DeadLetterQueue.{CommandType}' to the configuration source and give it a value.
        /// </remarks>
        /// <returns>
        /// The dead letter queue.
        /// </returns>
        public string GetDeadLetterQueueForCommand(Command command)
        {
            return GetValueForCommandType(
                command: command,
                keyFormat: "ServiceBus.MsmqClient.DeadLetterQueue.{0}",
                allowMissingConfig: true);
        }


        private string GetValueForCommandType(
                                            Command command,
                                            string keyFormat,
                                            bool allowMissingConfig)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var key = string.Format(keyFormat, command.GetType().Name);

            string value = null;
            var type = command.GetType();

            while (value == null && type.BaseType != null)
            {
                value = _configurationReader.GetStringValue(string.Format(keyFormat, type.Name));
                type = type.BaseType;
            }

            if (value == null && allowMissingConfig)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException($"Could not find configuration for key '{key}'");

            return value;
        }
    }
}
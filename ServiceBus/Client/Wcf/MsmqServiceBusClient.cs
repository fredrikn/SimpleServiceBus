namespace ServiceBus.Client.Wcf
{
    using System;
    using System.ServiceModel;
    using System.Transactions;
    using Configuration;
    using Contracts;
    using Hosts.Wcf;
    using Infrastructure.Msmq;

    /// <summary>
    /// A WCF basic HTTP service bus client.
    /// </summary>
    public class MsmqServiceBusClient : IServiceBusClient
    {
        private readonly MsmqSetup _msmqSetup = new MsmqSetup();
        private readonly IWcfChannelFactory<ICommandReceiver> _channelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqServiceBusClient"/> class. 
        /// </summary>
        public MsmqServiceBusClient()
        {
            Configuration = new MsmqConfiguration();
            _channelFactory = new WcfChannelFactory<ICommandReceiver>();
        }


        internal MsmqServiceBusClient(
            IWcfChannelFactory<ICommandReceiver> channelFactory,
            IMsmqConfiguration msmqConfiguration)
        {
            Configuration = msmqConfiguration;
            _channelFactory = channelFactory;
        }


        /// <summary>
        /// Gets or sets the configuration of the client.
        /// </summary>
        public IMsmqConfiguration Configuration { get; set; }


        /// <summary>
        /// The Send method will send the specified command to the host for handling.
        /// </summary>
        /// <param name="command">The command to be handled.</param>
        /// <remarks>
        /// For the client should know which endpoint address it will send the Command to
        /// you need to check the configuraiton <see cref="IMsmqConfiguration"/> used for this client 
        /// for more details.
        /// A deadletter queue for a specific command can be used if it's added to the 
        /// configuration. The configuration may be different based on the implementation of 
        /// the <see cref="IMsmqConfiguration"/>.
        /// </remarks>
        public void SendCommand(Command command)
        {
            var endpointAddress = Configuration.GetEndpointAddressForCommand(command);
            SendCommand(command, endpointAddress);
        }


        /// <summary>
        /// Send a Command to a host to be handled.
        /// </summary>
        /// <param name="command">The command to be handled.</param>
        /// <param name="hostQueueName">
        /// The Servicebus Wcf MSQM host queue name.
        /// </param>
        /// <remarks>
        /// A deadletter queue for a specific command can be used if it's added to the 
        /// configuration. The configuration may be different based on the implementation of 
        /// the <see cref="IMsmqConfiguration"/>.
        /// </remarks>
        public void SendCommand(Command command, string hostQueueName)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(hostQueueName))
                throw new ArgumentNullException(nameof(hostQueueName));

            var deadLetterQueueName = Configuration.GetDeadLetterQueueForCommand(command);

            InvokeReceiver(
                           GetMsqmHostEndoint(hostQueueName),
                           deadLetterQueueName,
                           r => r.ReceiveCommand(command));
        }


        /// <summary>
        /// Dispose the client.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }


        /// <summary>
        /// Dispose the client.
        /// </summary>
        /// <param name="disposing">
        /// Specifies if Dispose should clean up managed code.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
        }


        private void InvokeReceiver(
                                    string endpointAddress,
                                    string deadLetterQueueName,
                                    Action<ICommandReceiver> receiverAction)
        {
            try
            {
                var binding = GetNetMsmqBindingWithoutDlq();
                SetupCustomDeadLetterQueueIfConfigured(binding, deadLetterQueueName);

                var serviceBusReceiver = _channelFactory.CreateChannel(
                    endpointAddress: new EndpointAddress(endpointAddress),
                    binding: binding);

                using (var trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    receiverAction(serviceBusReceiver);
                    trans.Complete();

                    _channelFactory.Close();
                }
            }
            catch (Exception e)
            {
                throw new CommunicationException(
                    $"Can't communicate with the remote MSMQ at endpoint '{endpointAddress}', check inner exception for more details.", e);
            }
        }

        private NetMsmqBinding GetNetMsmqBindingWithoutDlq()
        {
            var netMsmqBinding = new NetMsmqBinding
            {
                MaxRetryCycles = Configuration.MaxRetryCycles,
                ReceiveRetryCount = Configuration.ReceiveRetryCount,
                RetryCycleDelay = Configuration.RetryCycleDelay,
                TimeToLive = Configuration.TimeToLive,
                Security = new NetMsmqSecurity { Mode = NetMsmqSecurityMode.None },
            };

            return netMsmqBinding;
        }


        private void SetupCustomDeadLetterQueueIfConfigured(
                                                            MsmqBindingBase netMsmqBinding,
                                                            string deadLetterQueueName)
        {
            if (string.IsNullOrWhiteSpace(deadLetterQueueName))
                return;

            _msmqSetup.EnsureQueueExists(deadLetterQueueName);

            netMsmqBinding.DeadLetterQueue = DeadLetterQueue.Custom;
            netMsmqBinding.CustomDeadLetterQueue = new Uri(
                $"net.msmq://localhost/private/{deadLetterQueueName}");
        }


        private string GetMsqmHostEndoint(string hostQueueName)
        {
            return $"net.msmq://{Configuration.MsmqServer}/private/{hostQueueName}";
        }
    }
}
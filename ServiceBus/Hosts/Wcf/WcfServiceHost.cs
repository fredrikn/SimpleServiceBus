namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.ServiceModel.Description;
    using Contracts;


    internal class WcfServiceHost : System.ServiceModel.ServiceHost
    {
        public WcfServiceHost(Type receiverType)
            : base(receiverType)
        {
        }

        protected override void OnOpening()
        {
            AddCustomDataContractSerializerToReceiveOperation();
            Description.Behaviors.Add(new ServiceHostBehavior());
            base.OnOpening();
        }


        private void AddCustomDataContractSerializerToReceiveOperation()
        {
            var contractDescription = Description.Endpoints[0].Contract;
            var receiveCommandOperationDescription = contractDescription.Operations.Find("ReceiveCommand");
            AddSerializerBehavior(receiveCommandOperationDescription, typeof(Command));
        }

        private static void AddSerializerBehavior(OperationDescription operationDescription, Type type)
        {
            var serializerBehavior = operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
            if (serializerBehavior == null)
            {
                serializerBehavior = new DataContractSerializerOperationBehavior(operationDescription);
                operationDescription.Behaviors.Add(serializerBehavior);
            }

            serializerBehavior.DataContractResolver = new WcfDataContractResolver(type);
        }
    }
}
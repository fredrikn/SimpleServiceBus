namespace ServiceBus.Hosts.Wcf
{
    using System.ServiceModel;
    using Contracts;

    /// <summary>
    /// The CommandReceiver interface for the service that will retrieve a Command.
    /// </summary>
    [ServiceKnownType("GetTypes", typeof(DataContractTypeLocator))]
    [ServiceContract]
    public interface ICommandReceiver
    {
        /// <summary>
        /// The method that will receive a Command.
        /// </summary>
        /// <param name="command">
        /// The received command.
        /// </param>
        [OperationContract(IsOneWay = true)]
        void ReceiveCommand(Command command);
    }
}
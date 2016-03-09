namespace ServiceBus.Hosts.Wcf
{
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The ServiceBusErrorHandler interface, used to handle exceptions from the ServiceBus. 
    /// </summary>
    public interface IServiceErrorHandler : IErrorHandler
    {
    }
}
namespace ServiceBus.Hosts.Wcf
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    internal class ServiceHostBehavior : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var errorHandler = ServiceBusHost.Config.Services.GetService<IServiceErrorHandler>();

            var channelDispatchers = serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>();
            foreach (var channelDispatcher in channelDispatchers)
            {
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }
    }
}
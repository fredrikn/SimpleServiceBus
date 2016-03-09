namespace ServiceBus.Client.Wcf
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;


    /// <summary>
    /// The WcfChannelFactory interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the service to create.
    /// </typeparam>
    public interface IWcfChannelFactory<out T>
    {
        /// <summary>
        /// Creates a Channel for service of type T.
        /// </summary>
        /// <param name="endpointAddress">
        /// The service endpoint address.
        /// </param>
        /// <param name="binding">
        /// The service binding.
        /// </param>
        /// <returns>
        /// The a service of type T.
        /// </returns>
        T CreateChannel(EndpointAddress endpointAddress, Binding binding);


        /// <summary>
        /// Close the ChannelFactory.
        /// </summary>
        void Close();
    }
}
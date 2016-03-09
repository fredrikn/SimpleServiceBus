namespace ServiceBus.Client.Wcf
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    internal sealed class WcfChannelFactory<T> : IWcfChannelFactory<T>, IDisposable
    {
        [ThreadStatic]
        private static ChannelFactory<T> _channelFactory;

        public T CreateChannel(EndpointAddress endpointAddress, Binding binding)
        {
            if (endpointAddress == null)
                throw new ArgumentNullException(nameof(endpointAddress));

            if (binding == null)
                throw new ArgumentNullException(nameof(binding));

            _channelFactory = new ChannelFactory<T>(binding);
            return _channelFactory.CreateChannel(endpointAddress);
        }


        public void Close()
        {
            _channelFactory?.Close();
        }


        public void Dispose()
        {
            Close();
        }
    }
}
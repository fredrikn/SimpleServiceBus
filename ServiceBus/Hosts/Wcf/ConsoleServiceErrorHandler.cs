namespace ServiceBus.Hosts.Wcf
{
    using System;
    using System.ServiceModel.Channels;

    public class ConsoleServiceErrorHandler : IServiceErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        public bool HandleError(Exception error)
        {
            Console.WriteLine(error);
            return false;
        }
    }
}
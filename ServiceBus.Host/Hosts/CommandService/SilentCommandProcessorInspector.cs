namespace ServiceBus.Hosts.CommandService
{
    using System;
    using Contracts;

    class SilentCommandProcessorInspector : ICommandProcessorInspector
    {
        public void BeforeProcessing(Command command)
        {
        }

        public void AfterProcessing(Command command)
        {
        }

        public void FailedProcessing(Command command, Exception exception)
        {
        }
    }
}

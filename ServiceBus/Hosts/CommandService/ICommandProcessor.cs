namespace ServiceBus.Hosts.CommandService
{
    using Contracts;

    public interface ICommandProcessor
    {
        void Handle(Command command);
    }
}
namespace ServiceBus.Host.TestHost.CommandHandlers
{
    using System;
    using Hosts.CommandService;
    using TestCommand;

    public class CreateUserCommandHandler : ICommandHandler<CreateUser>
    {
        public void Handle(CreateUser command)
        {
            Console.WriteLine(command.Name);
        }
    }
}
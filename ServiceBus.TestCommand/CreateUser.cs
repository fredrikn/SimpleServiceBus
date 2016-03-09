using ServiceBus.Contracts;

namespace ServiceBus.TestCommand
{
    public class CreateUser : Command
    {
        public string Name { get; set; }
    }
}

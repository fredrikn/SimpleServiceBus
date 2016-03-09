using ServiceBus.TestCommand;

namespace ServiceBus.Client.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bus = ServiceBus.Create<Startup>())
            {
                bus.Send(new CreateUser() {Name = "John Doe"}); // "ServiceBus.Host.TestHost");
            }
        }
    }
}
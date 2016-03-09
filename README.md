# Simple Service Bus

Create a Command:
``` C#
    public class CreateUser : Command
    {
        public string Name { get; set; }
    }
```
Create a Client that sends the command through the ServiceBus:

``` C#
    using (var bus = ServiceBus.Create<Startup>())
    {
        bus.Send(new CreateUser() {Name = "John Doe"});
    }
```
Client configuration Startup.cs:

``` C#
    class Startup : IStartup
    {
        public void Configuration(IClientConfig config)
        {
            //config.UseWcfMsmq();
            config.UseWcfBasicHttp();
        }
    }
```

Create a server/host and add a command handler for the command

Command handler example:
``` C#
    public class CreateUserCommandHandler : ICommandHandler<CreateUser>
    {
        public void Handle(CreateUser command)
        {
            Console.WriteLine(command.Name);
        }
    }
```
Host:
```C#
    using (ServiceBusHost.Start<Startup>())
    {
        Console.WriteLine("Running. Press ENTER to close exit.");
        Console.ReadLine();
    }
```

Host configuration, Startup.cs

``` C#
    public class Startup : IStartup
    {
        public void Configuration(IHostConfig config)
        {
            //config.UseWcfMsmq();
            config.UseWcfBasicHttp();
        }
    }
```

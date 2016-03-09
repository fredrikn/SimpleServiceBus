namespace ServiceBus.Host.TestHost
{
    using System;
    using Hosts;


    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceBusHost.Start<Startup>())
            {
                Console.WriteLine("Running. Press ENTER to close exit.");
                Console.ReadLine();
            }
        }
    }
}
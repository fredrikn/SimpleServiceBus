namespace ServiceBus.Infrastructure
{
    using System.Reflection;

    internal class AssemblyLoader : IAssemblyLoader
    {
        public Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }
    }
}

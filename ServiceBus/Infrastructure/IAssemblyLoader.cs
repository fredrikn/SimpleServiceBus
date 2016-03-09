namespace ServiceBus.Infrastructure
{
    using System.Reflection;

    /// <summary>
    /// A class for wrapping Assembly.GetEntryAssembly.
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Gets the entry assembly.
        /// </summary>
        /// <returns>The entry assembly.</returns>
        Assembly GetEntryAssembly();
    }
}

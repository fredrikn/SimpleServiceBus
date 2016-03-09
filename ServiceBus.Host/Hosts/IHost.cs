namespace ServiceBus.Hosts
{
    public interface IHost
    {
        /// <summary>
        /// Opens this host.
        /// </summary>
        void Open();

        /// <summary>
        /// Closes this host.
        /// </summary>
        void Close();
    }
}
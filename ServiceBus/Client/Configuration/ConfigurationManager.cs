namespace ServiceBus.Client.Configuration
{
    using System;

    internal class ConfigurationManager
    {
        public void Config<TStartUp>(IClientConfig config) where TStartUp : IStartup, new()
        {
            try
            {
                new TStartUp().Configuration(config);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    $"The configuration class '{typeof (TStartUp).FullName}' can't invoke the Configuration method, see inner exception for more details.",
                    e);
            }
        }
    }
}
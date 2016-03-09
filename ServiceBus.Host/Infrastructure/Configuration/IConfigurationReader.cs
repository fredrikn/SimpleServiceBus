namespace ServiceBus.Infrastructure.Configuration
{
    using System;

    /// <summary>
    /// The ConfigurationReader used to read configuration settings.
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// The get value return a configured value based on the specified key.
        /// </summary>
        /// <param name="key">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// Returns the value configured value.
        /// </returns>
        string GetStringValue(string key);

        /// <summary>
        /// Gets a numeric value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A numeric value, or null if the key is not present.</returns>
        int? GetNumericValue(string key);

        /// <summary>
        /// Gets a time span value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A time span value, or null if the key is not present.</returns>
        TimeSpan? GetTimeSpanValue(string key);
    }
}
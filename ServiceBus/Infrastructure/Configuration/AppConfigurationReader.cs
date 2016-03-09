namespace ServiceBus.Infrastructure.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// A App.config or Web.config reader.
    /// </summary>
    public class AppConfigReader : IConfigurationReader
    {
        /// <summary>
        /// Gets a configuration value based on the specified key.
        /// </summary>
        /// <param name="key">
        /// The key to get a configuration value form.
        /// </param>
        /// <returns>
        /// The configuration value associated with the key.
        /// </returns>
        public string GetStringValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Gets a numeric value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// A numeric value, or null if the key is not present.
        /// </returns>
        public int? GetNumericValue(string key)
        {
            var value = GetStringValue(key);

            if (string.IsNullOrWhiteSpace(value))
                return null;

            int numericValue;

            if (!int.TryParse(value, out numericValue))
            {
                throw new ConfigurationErrorsException(
                    $"The configured value '{value}' for the key '{key}' is not a numeric value.");
            }

            return numericValue;
        }

        /// <summary>
        /// Gets a time span value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// A time span value, or null if the key is not present.
        /// </returns>
        public TimeSpan? GetTimeSpanValue(string key)
        {
            var value = GetStringValue(key);

            if (string.IsNullOrWhiteSpace(value))
                return null;

            TimeSpan timeSpanValue;

            if (!TimeSpan.TryParse(value, out timeSpanValue))
            {
                throw new ConfigurationErrorsException(
                    $"The configured value '{value}' for the key '{key}' is not a TimeSpan value like '00:00:00', hour:minutes:seconds.");
            }

            return timeSpanValue;
        }
    }
}
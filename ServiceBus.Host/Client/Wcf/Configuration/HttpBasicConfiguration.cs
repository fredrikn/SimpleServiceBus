namespace ServiceBus.Client.Wcf.Configuration
{
    using System;
    using Infrastructure.Configuration;

    public class HttpBasicConfiguration : IHttpBasicConfiguration
    {
        private Uri _uri;

        private readonly IConfigurationReader _configurationReader = new AppConfigReader();


        /// <summary>
        /// Gets or sets the Url of the WCF Command reciever service. Default is http://localhost:8080/Command.
        /// The Url can also be configure in the applicaiton configuraiton file under section appSetttings,
        /// use the key 'httpBasicHttpHostUrl'.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (_uri != null)
                    return _uri;

                var url = _configurationReader.GetStringValue("httpBasicHttpHostUrl");

                _uri = string.IsNullOrWhiteSpace(url) ? new Uri("http://localhost:8080/Command") : new Uri(url);

                return _uri;
            }
            set
            {
                _uri = value;
            }
        }
    }
}
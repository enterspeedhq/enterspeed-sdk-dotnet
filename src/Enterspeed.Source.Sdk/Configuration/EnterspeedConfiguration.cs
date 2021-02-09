namespace Enterspeed.Source.Sdk.Configuration
{
    public class EnterspeedConfiguration
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets timeout in seconds.
        /// </summary>
        public int ConnectionTimeout => 60;
    }
}

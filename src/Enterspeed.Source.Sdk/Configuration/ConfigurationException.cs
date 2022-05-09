using System;
namespace Enterspeed.Source.Sdk.Configuration
{
    public class ConfigurationException : Exception
    {
        private ConfigurationException()
        {
        }

        public ConfigurationException(string parameterName)
            : base($"Missing {parameterName}")
        {
        }
    }
}

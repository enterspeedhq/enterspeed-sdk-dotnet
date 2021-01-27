using System.Collections.Generic;

namespace Enterspeed.Source.Sdk.Api.Models.Properties
{
    public class ObjectEnterspeedProperty : IEnterspeedProperty
    {
        public string Name { get; }
        public string Type => "object";
        public IDictionary<string, IEnterspeedProperty> Properties { get; }

        public ObjectEnterspeedProperty(string name, IDictionary<string, IEnterspeedProperty> properties)
        {
            Name = name;
            Properties = properties;
        }

        public ObjectEnterspeedProperty(IDictionary<string, IEnterspeedProperty> properties)
        {
            Properties = properties;
        }
    }
}

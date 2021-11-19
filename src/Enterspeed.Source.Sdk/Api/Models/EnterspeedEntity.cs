using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Sdk.Api.Models
{
    public class EnterspeedEntity : IEnterspeedEntity
    {
        public string Id { get; }
        public string Type { get; }
        public string Url { get; }
        public string[] Redirects { get; }
        public string ParentId { get; }
        public IDictionary<string, IEnterspeedProperty> Properties { get; }
    }
}

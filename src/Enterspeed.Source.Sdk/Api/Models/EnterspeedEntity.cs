using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Sdk.Api.Models
{
    public class EnterspeedEntity : IEnterspeedEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public IDictionary<string, IEnterspeedProperty> Properties { get; set; }
    }
}

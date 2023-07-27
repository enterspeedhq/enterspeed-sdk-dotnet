using System.Collections.Generic;

namespace Enterspeed.Source.Sdk.Api.Models
{
    internal class EnterspeedJsonIngestEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public IDictionary<string, object> Properties { get; set; }

        public EnterspeedJsonIngestEntity(IEnterspeedJsonEntity entity, IDictionary<string, object> properties)
        {
            Id = entity.Id;
            Type = entity.Type;
            Url = entity.Url;
            Redirects = entity.Redirects;
            ParentId = entity.ParentId;
            Properties = properties;
        }
    }
}
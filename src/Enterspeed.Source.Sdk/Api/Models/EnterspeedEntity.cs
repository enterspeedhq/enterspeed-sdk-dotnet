namespace Enterspeed.Source.Sdk.Api.Models
{
    public class EnterspeedEntity : IEnterspeedEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public object Properties { get; set; }

        public EnterspeedEntity(string id, string type, object properties = default)
        {
            Id = id;
            Type = type;
            Properties = properties;
        }
    }

    public class EnterspeedEntity<T> : IEnterspeedEntity<T>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public T Properties { get; set; }

        public EnterspeedEntity(string id, string type, T properties = default)
        {
            Id = id;
            Type = type;
            Properties = properties;
        }
    }

    internal class EnterspeedEntityV2<T> : IEnterspeedEntityV2<T>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string OriginParentId { get; set; }
        public T Properties { get; set; }

        public EnterspeedEntityV2(IEnterspeedEntity<T>  enterspeedEntity)
        {
            Id = enterspeedEntity.Id;
            Type = enterspeedEntity.Type;
            Url = enterspeedEntity.Url;
            Redirects = enterspeedEntity.Redirects;
            OriginParentId = enterspeedEntity.ParentId;
            Properties = enterspeedEntity.Properties;
        }
    }
}

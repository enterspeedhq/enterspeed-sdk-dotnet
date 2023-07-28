namespace Enterspeed.Source.Sdk.Api.Models
{
    public class EnterspeedSourceEntity : IEnterspeedSourceEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public object Properties { get; set; }

        public EnterspeedSourceEntity(string id, string type, object properties = default)
        {
            Id = id;
            Type = type;
            Properties = properties;
        }
    }

    public class EnterspeedSourceEntity<T> : IEnterspeedSourceEntity<T>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public T Properties { get; set; }

        public EnterspeedSourceEntity(string id, string type, T properties = default)
        {
            Id = id;
            Type = type;
            Properties = properties;
        }
    }
}
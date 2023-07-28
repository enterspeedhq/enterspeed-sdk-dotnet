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

        public EnterspeedSourceEntity(string id, string type, object properties = null)
        {
            Id = id;
            Type = type;
            Properties = properties;
        }
    }
}
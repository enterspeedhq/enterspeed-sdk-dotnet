namespace Enterspeed.Source.Sdk.Api.Models
{
    public class EnterspeedJsonEntity : IEnterspeedJsonEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public string Properties { get; set; }
    }
}
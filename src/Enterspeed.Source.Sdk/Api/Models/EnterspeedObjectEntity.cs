namespace Enterspeed.Source.Sdk.Api.Models
{
    public class EnterspeedObjectEntity : IEnterspeedObjectEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public object Properties { get; set; }
    }
}
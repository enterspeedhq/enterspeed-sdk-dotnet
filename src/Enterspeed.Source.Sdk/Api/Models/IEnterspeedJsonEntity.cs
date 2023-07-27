namespace Enterspeed.Source.Sdk.Api.Models
{
    public interface IEnterspeedJsonEntity
    {
        string Id { get; }
        string Type { get; }
        string Url { get; }
        string[] Redirects { get; }
        string ParentId { get; }
        /// <summary>
        /// Must be a serialized json object containing data properties for your source entity
        /// </summary>
        string Properties { get; }
    }
}
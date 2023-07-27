namespace Enterspeed.Source.Sdk.Api.Models
{
    public interface IEnterspeedObjectEntity
    {
        string Id { get; }
        string Type { get; }
        string Url { get; }
        string[] Redirects { get; }
        string ParentId { get; }
        /// <summary>
        /// Any object containing data properties for your source entity
        /// </summary>
        object Properties { get; }
    }
}
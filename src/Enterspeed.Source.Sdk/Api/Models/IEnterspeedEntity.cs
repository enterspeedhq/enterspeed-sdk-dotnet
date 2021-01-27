using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Sdk.Api.Models
{
    public interface IEnterspeedEntity
    {
        string Id { get; }
        string Type { get; }
        string Url { get; }
        string[] Redirects { get; }
        string ParentId { get; }
        IDictionary<string, IEnterspeedProperty> Properties { get; }
    }
}

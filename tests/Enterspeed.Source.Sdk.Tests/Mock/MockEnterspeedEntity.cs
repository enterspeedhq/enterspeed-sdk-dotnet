using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Sdk.Tests.Mock
{
    public class MockEnterspeedEntity : IEnterspeedEntity<Dictionary<string, IEnterspeedProperty>>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string[] Redirects { get; set; }
        public string ParentId { get; set; }
        public Dictionary<string, IEnterspeedProperty> Properties { get; set; }
    }
}
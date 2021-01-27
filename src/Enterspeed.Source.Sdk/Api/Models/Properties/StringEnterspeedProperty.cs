namespace Enterspeed.Source.Sdk.Api.Models.Properties
{
    public class StringEnterspeedProperty : IEnterspeedProperty
    {
        public string Name { get; }
        public string Type => "string";
        public string Value { get; set; }

        public StringEnterspeedProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public StringEnterspeedProperty(string value)
        {
            Value = value;
        }
    }
}

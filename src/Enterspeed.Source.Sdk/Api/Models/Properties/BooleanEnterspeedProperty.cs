namespace Enterspeed.Source.Sdk.Api.Models.Properties
{
    public class BooleanEnterspeedProperty : IEnterspeedProperty
    {
        public string Name { get; }
        public string Type => "boolean";
        public bool Value { get; }

        public BooleanEnterspeedProperty(string name, bool value)
        {
            Name = name;
            Value = value;
        }

        public BooleanEnterspeedProperty(bool value)
        {
            Value = value;
        }
    }
}

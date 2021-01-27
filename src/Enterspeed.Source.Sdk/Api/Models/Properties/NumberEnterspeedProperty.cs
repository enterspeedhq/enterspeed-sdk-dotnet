using System;
using System.Globalization;

namespace Enterspeed.Source.Sdk.Api.Models.Properties
{
    public class NumberEnterspeedProperty : IEnterspeedProperty
    {
        public string Name { get; }
        public string Type => "number";
        public double Value { get; }

        private int? _precision;

        public int Precision
        {
            get
            {
                if (!_precision.HasValue)
                {
                    var numberParts = Value.ToString("R", CultureInfo.InvariantCulture).Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    if (numberParts.Length <= 1)
                    {
                        _precision = 0;
                    }
                    else
                    {
                        var decimalPart = numberParts[1];
                        _precision = decimalPart.Length;
                    }
                }

                return _precision.Value;
            }
        }

        public NumberEnterspeedProperty(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public NumberEnterspeedProperty(double value)
        {
            Value = value;
        }
    }
}

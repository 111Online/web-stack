using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Features.Values
{
    public class FeatureValue : IFeatureValue
    {
        public FeatureValue(string value)
        {
                Value = value;
        }

        public string Value { get; private set; }
    }
}

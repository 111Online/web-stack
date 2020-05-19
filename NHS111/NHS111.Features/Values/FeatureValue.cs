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

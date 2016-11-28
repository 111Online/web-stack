namespace NHS111.Features {
    public interface IFeature
    {
        bool IsEnabled { get; }
        string StringValue { get; }
    }
}
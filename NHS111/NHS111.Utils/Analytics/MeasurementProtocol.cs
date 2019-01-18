namespace NHS111.Utils.Analytics
{
    public enum HitType
    {
        PageView,
        ScreenView,
        Event,
        Transaction,
        Item,
        Social,
        Exception,
        Timing
    }

    public abstract class MeasurementProtocol
    {
        protected MeasurementProtocol(string trackingId) : this(trackingId, HitType.Event)
        {
        }

        protected MeasurementProtocol(string trackingId, HitType hitType)
        {
            Version = "1";
            TrackingId = trackingId;
            HitType = hitType;
        }

        public string Version { get; set; }

        public string TrackingId { get; set; }

        public string ClientId { get; set; }

        public string UserId { get; set; }

        public HitType HitType { get; set; }

        public abstract string Payload { get; }

        public abstract void PostToAnalyticsAsync(string requestUri);
    }
}

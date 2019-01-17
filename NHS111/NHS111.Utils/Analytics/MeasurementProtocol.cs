namespace NHS111.Utils.Analytics
{
    public abstract class MeasurementProtocol
    {
        protected MeasurementProtocol(string trackingId)
        {
            Version = "1";
            TrackingId = trackingId;
        }

        public string Version { get; set; }

        public string TrackingId { get; set; }

        public string ClientId { get; set; }

        public string UserId { get; set; }

        public abstract string Payload { get; }

        public abstract void PostToAnalyticsAsync(string requestUri);
    }
}

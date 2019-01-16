namespace NHS111.Utils.Analytics
{
    public abstract class MeasurementProtocol
    {
        protected MeasurementProtocol()
        {
            Version = "1";
        }

        public string Version { get; set; }

        public string TrackingId { get; set; }

        public string ClientId { get; set; }

        public string UserId { get; set; }

        public abstract string Payload { get; }

        public abstract void PostToAnalyticsAsync(string requestUri);
    }
}

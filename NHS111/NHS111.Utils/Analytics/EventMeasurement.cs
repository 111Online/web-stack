using System;
using System.Net.Http;
using NHS111.Utils.Filters;

namespace NHS111.Utils.Analytics
{
    public class EventMeasurement : MeasurementProtocol
    {
        public EventMeasurement(string trackingId, string clientId, HitType hitType) : base(trackingId, hitType)
        {
            ClientId = clientId;
        }

        public string EventCategory { get; set; }
        public string EventAction { get; set; }
        public string EventLabel { get; set; }
        public string Campaign { get; set; }
        public string Source { get; set; }
        public string DocumentPath { get; set; }
        public override string Payload
        {
            get { return GetPayload(); }
        }

        private string GetPayload()
        {
            switch (HitType)
            {
                case HitType.PageView:
                    return string.Format("v={0}&t=pageview&tid={1}&cid={2}&uid={3}&cn={4}&cs={5}&dp={6}&cm=Direct", Version, TrackingId, ClientId, UserId, Campaign, Source, DocumentPath);
                case HitType.Event:
                    return string.Format("v={0}&t=event&tid={1}&cid={2}&uid={3}&ec={4}&ea={5}&el={6}", Version, TrackingId, ClientId, UserId, EventCategory, EventAction, EventLabel);
                default:
                    throw new ArgumentOutOfRangeException(string.Format("A measurement protocol has not been defined for hit type {0}", HitType));
            }
            
        }

        public override async void PostToAnalyticsAsync(string requestUri)
        {
            var client = new HttpClient();
            await client.PostAsync(new Uri(string.Format("{0}/collect", requestUri), UriKind.Absolute), new StringContent(Payload));
        }
    }
}

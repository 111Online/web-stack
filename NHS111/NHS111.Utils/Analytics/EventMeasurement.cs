using System;
using System.Net.Http;
using NHS111.Utils.Filters;

namespace NHS111.Utils.Analytics
{
    public class EventMeasurement : MeasurementProtocol
    {
        public EventMeasurement(string trackingId, string clientId, string eventCategory) : base(trackingId)
        {
            EventCategory = eventCategory;
            ClientId = clientId;
        }

        public string EventCategory { get; set; }

        public string EventAction { get; set; }

        public string EventLabel { get; set; }
        public string Campaign { get; set; }
        public string Source { get; set; }

        public override string Payload
        {
            get { return string.Format("v={0}&t=event&tid={1}&cid={2}&uid={3}&ec={4}&ea={5}&el={6}&cn={7}&cs={8}", Version, TrackingId, ClientId, UserId, EventCategory, EventAction, EventLabel, Campaign, Source); }
        }

        public override async void PostToAnalyticsAsync(string requestUri)
        {
            var client = new HttpClient();
            await client.PostAsync(new Uri(string.Format("{0}/collect", requestUri), UriKind.Absolute), new StringContent(Payload));
        }
    }
}

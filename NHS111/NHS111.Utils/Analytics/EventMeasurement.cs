using System;
using System.Net.Http;

namespace NHS111.Utils.Analytics
{
    public class EventMeasurement : MeasurementProtocol
    {

        public string EventCategory { get; set; }

        public string EventAction { get; set; }

        public string EventLabel { get; set; }

        public override string Payload
        {
            get { return string.Format("v={0}&t=event&tid={1}&cid={2}&uid={3}&ec={4}&ea={5}&el={6}", Version, TrackingId, ClientId, UserId, EventCategory, EventAction, EventLabel); }
        }

        public override async void PostToAnalyticsAsync(string requestUri)
        {
            var client = new HttpClient();
            await client.PostAsync(new Uri(string.Format("{0}/collect", requestUri), UriKind.Absolute), new StringContent(Payload));
        }
    }
}

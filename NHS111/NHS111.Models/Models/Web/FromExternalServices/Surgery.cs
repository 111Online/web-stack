using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class Surgery
    {
        [JsonProperty(PropertyName = "address1")]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }

        [JsonProperty(PropertyName = "address3")]
        public string Address3 { get; set; }

        [JsonProperty(PropertyName = "address4")]
        public string Address4 { get; set; }

        [JsonProperty(PropertyName = "address5")]
        public string Address5 { get; set; }

        [JsonProperty(PropertyName = "closeDate")]
        [JsonConverter(typeof(DateTimeDataConverter))]
        public DateTime? CloseDate { get; set; }

        [JsonProperty(PropertyName = "contactPhone")]
        public string ContactPhone { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "openDate")]
        [JsonConverter(typeof(DateTimeDataConverter))]
        public DateTime? OpenDate { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }

        [JsonProperty(PropertyName = "prescribingSettings")]
        public string PrescribingSettings { get; set; }

        [JsonProperty(PropertyName = "statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty(PropertyName = "subtypeCode")]
        public string SubtypeCode { get; set; }

        [JsonProperty(PropertyName = "surgeryId")]
        public string SurgeryId { get; set; }
    }

    class DateTimeDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (string.IsNullOrEmpty(token.Value<string>()))
                return null;
            var year = Convert.ToInt32(token.ToString().Substring(0, 4));
            var month = Convert.ToInt32(token.ToString().Substring(4, 2));
            var day = Convert.ToInt32(token.ToString().Substring(6, 2));

            return new DateTime(year, month, day);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

}
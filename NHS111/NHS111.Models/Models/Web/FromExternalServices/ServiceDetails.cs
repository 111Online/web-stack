namespace NHS111.Models.Models.Web.FromExternalServices {
    using Newtonsoft.Json;

    public class ServiceDetails
    {
        [JsonProperty(PropertyName = "idField")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "odsCodeField")]
        public string OdsCode { get; set; }

        [JsonProperty(PropertyName = "nameField")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "contactDetailsField")]
        public ContactDetails[] ContactDetails { get; set; }
    }
}
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class ContactDetails
    {
        [JsonProperty(PropertyName = "tagField")]
        public ContactType Tag { get; set; }

        [JsonProperty(PropertyName = "nameField")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "valueField")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "orderField")]
        public int Order { get; set; }
    }

    public enum ContactType
    {

        /// <remarks/>
        dts,

        /// <remarks/>
        itk,

        /// <remarks/>
        telno,

        /// <remarks/>
        email,

        /// <remarks/>
        faxno,
    }
}

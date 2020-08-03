using Newtonsoft.Json;

namespace NHS111.Models.Models.Business.MicroSurvey
{
    public class EmbeddedData
    {
        [JsonProperty(PropertyName = "journey_id")]
        public string JourneyId { get; set; }
        [JsonProperty(PropertyName = "dx_code")]
        public string DxCode { get; set; }
        [JsonProperty(PropertyName = "dispo_date")]
        public string DispositionDate { get; set; }
        [JsonProperty(PropertyName = "dispo_time")]
        public string DispositionTime { get; set; }
        [JsonProperty(PropertyName = "ccg")]
        public string Ccg { get; set; }
        [JsonProperty(PropertyName = "micro_launch_page")]
        public string LaunchPage { get; set; }
        [JsonProperty(PropertyName = "validation_callback_offered")]
        public string ValidationCallbackOfferd { get; set; }
        [JsonProperty(PropertyName = "svcs_offered")]
        public string ServicesOffered { get; set; }
        [JsonProperty(PropertyName = "svce_count")]
        public string ServiceCount { get; set; }
        [JsonProperty(PropertyName = "rec_service_dos_type")]
        public string RecommendedServiceDosType { get; set; }
        [JsonProperty(PropertyName = "rec_service_id")]
        public string RecommendedServiceId { get; set; }
        [JsonProperty(PropertyName = "rec_service_name")]
        public string ResommendedServiceName { get; set; }
        [JsonProperty(PropertyName = "rec_service_alias_type")]
        public string RecommendedServiceAlias { get; set; }
        [JsonProperty(PropertyName = "rec_service_distance")]
        public string RecommendedServiceDistance { get; set; }
        [JsonProperty(PropertyName = "sd_code")]
        public string SdCode { get; set; }
        [JsonProperty(PropertyName = "sd_description")]
        public string SdDescription { get; set; }
        [JsonProperty(PropertyName = "sg_code")]
        public string SgCode { get; set; }
        [JsonProperty(PropertyName = "sg_description")]
        public string SgDescription { get; set; }
    }

}

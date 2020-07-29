using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.MicroSurvey
{
    public class SurveyResult
    {
        [JsonProperty("values")]
        public SurveyResultValues Values { get; set; }
    }
}

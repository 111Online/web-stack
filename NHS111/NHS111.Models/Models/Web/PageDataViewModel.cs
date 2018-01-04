using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NHS111.Models.Models.Web
{
    public class PageDataViewModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PageType
        {
            ModuleZero = 0,
            Demographics,
            Search,
            SearchResults,
            Categories,
            FirstQuestion,
            Question
        }

        public PageType Page { get; set; }
        public string TxNumber { get; set; }
        public string QuestionId { get; set; }
        public string StartingPathwayNo { get; set; }
        public string StartingPathwayTitle { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string PathwayNo { get; set; }
        public string PathwayTitle { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DxCode { get; set; }
        public string SearchString { get; set; }
        public string Campaign { get; set; }
        public string Source { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

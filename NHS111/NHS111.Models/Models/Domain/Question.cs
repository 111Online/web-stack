using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class Question
    {

        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }

        [JsonProperty(PropertyName = "topic")]
        public string Topic { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "questionNo")]
        public string QuestionNo { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "jtbs")]
        public string Jtbs { get; set; }

        [JsonProperty(PropertyName = "jtbsText")]
        public string JtbsText { get; set; }

        [JsonProperty(PropertyName = "rationale")]
        public string Rationale { get; set; }

        [JsonProperty(PropertyName = "timeFrameText")]
        public string TimeFrameText { get; set; }

        [JsonProperty(PropertyName = "timeFrame")]
        public int TimeFrame { get; set; }

        [JsonProperty(PropertyName = "waitTimeText")]
        public string WaitTimeText { get; set; }

        [JsonProperty(PropertyName = "dispositionUrgencyText")]
        public string DispositionUrgencyText { get; set; }

        [JsonProperty(PropertyName = "reportText")]
        public string ReportText { get; set; }

        [JsonProperty(PropertyName = "careAdviceId")]
        public string CareAdviceId { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "excludeKeywords")]
        public string ExcludeKeywords { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [JsonProperty(PropertyName = "buttonText")]
        public string NextButtonText { get; set; }

        [JsonProperty(PropertyName = "questionType")]
        public QuestionType QuestionType { get; set; }


        public bool IsJustToBeSafe()
        {
            return !(string.IsNullOrEmpty(Jtbs) || Jtbs.EndsWith("-"));
        }

        public Tuple<string, List<string>> JtbsTextWithBullets()
        {
            return WithBullets(JtbsText, false);
        }

        public Tuple<string, List<string>> TitleWithBullets()
        {
            return WithBullets(Title, true);
        }

        private static Tuple<string, List<string>> WithBullets(string text, bool keepQuestiomMark)
        {
            var parts = Regex.Split(text ?? "", @"(?=(\d+\.\s))").
                Where((item, index) => index % 2 == 0).
                Select((item, index) => (index > 0 || !keepQuestiomMark) ? item.Trim().Trim('?').Trim() : item.Trim()).
                ToList();

            return new Tuple<string, List<string>>(parts.First(), parts.Skip(1).ToList());
        }
    }
    public enum QuestionType
    {
        Group = 0,
        Boolean = 2,
        Date = 5,
        String = 8,
        Text = 9,
        Choice = 11,
        Attachment = 13
    }
}
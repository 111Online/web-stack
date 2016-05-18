using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using NHS111.Models.Mappers;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web
{
    public class JourneyViewModel
    {
        public Guid UserId { get; set; }
        public string PathwayId { get; set; }
        public string PathwayNo { get; set; }
        public string PathwayTitle { get; set; }
        public string Id { get; set; }

        private string _title;
        public string Title
        {
            get { return string.IsNullOrEmpty(_title) ? "" : _title; }
            set { _title = value; }
        }
        public string TitleWithoutBullets { get; set; }
        public List<string> Bullets { get; set; }

        public string Rationale { get; set; }
        public string RationaleHtml {
            get { return StaticTextToHtml.Convert(Rationale); }
        }

        public List<Answer> Answers { get; set; }
        public string SelectedAnswer { get; set; }
        public NodeType NodeType { get; set; }

        public string JourneyJson { get; set; }

        public UserInfo UserInfo { get; set; }

        public string PreviousTitle { get; set; }
        public string PreviousStateJson { get; set; }
        public string QuestionNo { get; set; }
        public string SymptomDiscriminator { get; set; }
        public IDictionary<string, string> State { get; set; }
        public string StateJson { get; set; }
        public KeywordBag CollectedKeywords { get; set; }

        public string StepLink {
            get {
                var age = UserInfo != null ? UserInfo.Age : 0;
                return string.Format("/question/direct/{0}/{1}/{2}/?answers={3}", PathwayId, age, PathwayTitle,
                    string.Join(",", GetPreviousAnswers()));
            }
        }

        private IEnumerable<int> GetPreviousAnswers() {
            var journey = JsonConvert.DeserializeObject<Journey>(JourneyJson);
            return journey.Steps.Select(step => step.Answer.Order - 1);
        }

        public JourneyViewModel()
        {
            Answers = new List<Answer>();
            JourneyJson = JsonConvert.SerializeObject(new Journey());
            Bullets = new List<string>();
            State = new Dictionary<string, string>();
            SymptomDiscriminator = String.Empty;
            CollectedKeywords = new KeywordBag();

        }

        public List<Answer> OrderedAnswers()
        {
            return Answers.OrderBy(x=>x.Order).ToList();
        }
    }
}
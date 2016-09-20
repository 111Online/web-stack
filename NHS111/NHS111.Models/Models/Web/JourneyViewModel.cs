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
        public Guid SessionId { get; set; }
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
        public Journey Journey { get; set; }

        public UserInfo UserInfo { get; set; }

        public bool IsFirstStep {
            get {
                if (string.IsNullOrEmpty(JourneyJson))
                    return false;
                return !JsonConvert.DeserializeObject<Journey>(JourneyJson).Steps.Any();
            }
        }

        public string PreviousStateJson { get; set; }
        public string QuestionNo { get; set; }
        public string SymptomDiscriminatorCode { get; set; }
        public IDictionary<string, string> State { get; set; }
        public string StateJson { get; set; }
        public KeywordBag CollectedKeywords { get; set; }
        public string TimeFrameText { get; set; }
        public OutcomeGroup OutcomeGroup { get; set; }

        private bool _displayOutcomeReferenceOnly = false;
        public bool DisplayOutcomeReferenceOnly
        {

            get { return _displayOutcomeReferenceOnly; }
            set { _displayOutcomeReferenceOnly = value; }
        }

        public string StepLink {
            get {
                var age = UserInfo != null ? UserInfo.Age : 0;
                return string.Format("/question/direct/{0}/{1}/{2}/?answers={3}", PathwayId, age, PathwayTitle,
                    string.Join(",", GetPreviousAnswers()));
            }
        }

        public string OutcomeDetailLink
        {
            get
            {
                var age = UserInfo != null ? UserInfo.Age : 0;
                return string.Format("/question/outcomedetail/{0}/{1}/{2}/?answers={3}", PathwayId, age, PathwayTitle,
                    string.Join(",", GetPreviousAnswers()));
            }
        }

        private IEnumerable<int> GetPreviousAnswers() {
            return Journey.Steps.Select(step => step.Answer.Order - 1);
        }

        public JourneyStep ToStep() {
            var answer = JsonConvert.DeserializeObject<Answer>(SelectedAnswer);
            return new JourneyStep {
                QuestionNo = QuestionNo,
                QuestionTitle = Title,
                Answer = answer,
                QuestionId = Id
            };
        }

        public JourneyViewModel()
        {
            Answers = new List<Answer>();
            JourneyJson = JsonConvert.SerializeObject(new Journey());
            Bullets = new List<string>();
            State = new Dictionary<string, string>();
            SymptomDiscriminatorCode = String.Empty;
            CollectedKeywords = new KeywordBag();

        }

        public List<Answer> OrderedAnswers()
        {
            return Answers.OrderBy(x=>x.Order).ToList();
        }

        public void ProgressState() {
            PreviousStateJson = StateJson;
            State = JsonConvert.DeserializeObject<Dictionary<string, string>>(StateJson);
        }


        public void RemoveLastStep() {
            Journey.RemoveLastStep();

            StateJson = PreviousStateJson;
            JourneyJson = JsonConvert.SerializeObject(Journey);
            State = JsonConvert.DeserializeObject<Dictionary<string, string>>(StateJson);
        }
    }
}
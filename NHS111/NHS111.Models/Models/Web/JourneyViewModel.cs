using System;
using System.Collections.Generic;
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
        public Guid JourneyId { get; set; }
        public string PathwayId { get; set; }
        public string PathwayNo { get; set; }
        public string PathwayTitle { get; set; }
        public string PathwayTraumaType { get; set; }
        public bool IsTraumaPathway
        {
            get { return !string.IsNullOrEmpty(PathwayTraumaType) && PathwayTraumaType.Equals("Trauma"); }
        }
        public string DigitalTitle { get; set; }
        public string Id { get; set; }
        public string EntrySearchTerm { get; set; }
        public string TriggerQuestionNo { get; set; }
        public string TriggerQuestionAnswer { get; set; }

        private string _title;
        public string Title
        {
            get { return string.IsNullOrEmpty(_title) ? "" : _title; }
            set { _title = value; }
        }
        public string TitleWithoutBullets { get; set; }
        public List<string> Bullets { get; set; }

        public string Rationale { get; set; }
        public string RationaleHtml
        {
            get { return StaticTextToHtml.Convert(Rationale); }
        }

        public List<Answer> Answers { get; set; }

        public NodeType NodeType { get; set; }

        public string JourneyJson { get; set; }
        public Journey Journey { get; set; }

        public UserInfo UserInfo { get; set; }

        public bool IsFirstStep
        {
            get
            {
                if (string.IsNullOrEmpty(JourneyJson))
                    return false;
                return !JsonConvert.DeserializeObject<Journey>(JourneyJson).Steps.Any();
            }
        }

        public IEnumerable<Pathway> CommonTopics { get; set; }

        public string QuestionNo { get; set; }
        public string SymptomDiscriminatorCode { get; set; }
        public IDictionary<string, string> State { get; set; }
        public string StateJson { get; set; }
        public KeywordBag CollectedKeywords { get; set; }
        public string TimeFrameText { get; set; }
        public OutcomeGroup OutcomeGroup { get; set; }
        public string WaitTimeText { get; set; }
        public DateTime DispositionTime { get; set; }
        public string DispositionUrgencyText { get; set; }

        public string DispositionUrgencyTitle
        {
            get
            {
                if (OutcomeGroup == null || string.IsNullOrEmpty(OutcomeGroup.Text))
                    return string.Empty;
                // convert to char array of the string
                char[] outcomeGroupArray = OutcomeGroup.Text.ToCharArray();
                // upper case the first char
                outcomeGroupArray[0] = char.ToUpper(outcomeGroupArray[0]);
                // return the array made of the new char array
                var outcomeGroupText = new string(outcomeGroupArray);

                return string.Format("{0} {1}", outcomeGroupText, DispositionUrgencyText);
            }
        }

        public int TimeFrameMinutes { get; set; }

        private bool _displayOutcomeReferenceOnly = false;
        public bool DisplayOutcomeReferenceOnly
        {

            get { return _displayOutcomeReferenceOnly; }
            set { _displayOutcomeReferenceOnly = value; }
        }

        public string StepLink
        {
            get
            {
                var age = UserInfo.Demography != null ? UserInfo.Demography.Age : 0;
                return string.Format("/question/direct/{0}/{1}/{2}/{3}/?answers={4}", PathwayId, age, PathwayTitle, CurrentPostcode, string.Join(",", GetPreviousAnswers()));
            }
        }

        public string OutcomeDetailLink
        {
            get
            {
                var age = UserInfo != null ? UserInfo.Demography.Age : 0;
                return string.Format("/question/outcomedetail/{0}/{1}/{2}/?answers={3}", PathwayId, age, PathwayTitle,
                    string.Join(",", GetPreviousAnswers()));
            }
        }

        public IEnumerable<string> PathwayNumbers { get; set; }
        public IEnumerable<CareAdvice> InlineCareAdvice { get; set; }
        public bool FilterServices { get; set; }

        private IEnumerable<int> GetPreviousAnswers()
        {
            if (Journey == null)
                return new List<int>();
            return Journey.Steps.Select(step => step.Answer.Order - 1);
        }

        public string UserZoomTitle { get; set; }
        public string UserZoomUrl { get; set; }
        public string Campaign { get; set; }
        public string Source { get; set; }
        public string CurrentPostcode { get; set; }
        public bool HasSeenPreamble { get; set; }


        public string FormattedCurrentPostcode
        {
            get
            {
                if (CurrentPostcode == null) return null;
                var normalisedPostcode = CurrentPostcode.Trim().Replace(" ", "").ToUpper();
                if (normalisedPostcode.Length < 4) return normalisedPostcode;
                return normalisedPostcode.Insert(normalisedPostcode.Length - 3, " ");
            }
        }

        public string Content { get; set; }

        public string NextButtonText { get; set; }

        public JourneyViewModel()
        {
            Answers = new List<Answer>();
            JourneyJson = JsonConvert.SerializeObject(new Journey());
            Bullets = new List<string>();
            State = new Dictionary<string, string>();
            SymptomDiscriminatorCode = String.Empty;
            CollectedKeywords = new KeywordBag();
            FilterServices = true;
            UserInfo = new UserInfo { CurrentAddress = new FindServicesAddressViewModel() };
        }

        public List<Answer> OrderedAnswers()
        {
            return Answers.OrderBy(x => x.Order).ToList();
        }

        public void ProgressState()
        {
            State = JsonConvert.DeserializeObject<Dictionary<string, string>>(StateJson);
        }


        public void RemoveLastStep()
        {
            StateJson = Journey.GetLastState();
            Journey.RemoveLastStep();
            JourneyJson = JsonConvert.SerializeObject(Journey);
            State = JsonConvert.DeserializeObject<Dictionary<string, string>>(StateJson);
        }
    }
}
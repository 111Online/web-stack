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
    public class JourneyViewModelEqualityComparer
        : IEqualityComparer<JourneyViewModel> {

        public bool ComparePathwayId { get; set; }
        public bool CompareAge { get; set; }
        public bool CompareSex { get; set; }
        public bool CompareQuestionNos { get; set; }
        public bool CompareAnswers { get; set; }

        public JourneyViewModelEqualityComparer() {
            ComparePathwayId = true;
            CompareAge = false;
            CompareSex = false;
            CompareQuestionNos = true;
            CompareAnswers = true;
        }

        public bool Equals(JourneyViewModel x, JourneyViewModel y) {
            //a lot of these checks can be pulled out into their respective types
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (ComparePathwayId && x.PathwayId != y.PathwayId)
                return false;

            if (CompareAge && !AgeEquals(x.UserInfo, y.UserInfo))
                return false;

            if (CompareSex && !SexEquals(x.UserInfo, y.UserInfo))
                return false;

            if ((!CompareQuestionNos && !CompareAnswers))
                return true; //we're done

            return JourneysEquals(x.Journey, y.Journey);
        }

        private bool JourneysEquals(Journey x, Journey y) {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Steps == null && y.Steps == null)
                return true;

            if (x.Steps == null || y.Steps == null)
                return false;

            if (!x.Steps.Any() && !y.Steps.Any())
                return true;

            for (var i = 0; i < x.Steps.Count; i++) {
                if (x.Steps[i] == null && y.Steps[i] == null)
                    continue; //no need to compare these two any further

                if (x.Steps[i] == null || y.Steps[i] == null)
                    return false;

                if (CompareQuestionNos && x.Steps[i].QuestionNo != y.Steps[i].QuestionNo)
                    return false;

                if (!CompareAnswers)
                    continue;

                //these checks could fold into Answer.Equals() for example
                if (x.Steps[i].Answer == null && y.Steps[i].Answer == null)
                    continue;

                if (x.Steps[i].Answer == null || y.Steps[i].Answer == null)
                    return false;

                if (x.Steps[i].Answer.Title != y.Steps[i].Answer.Title)
                    return false;
            }

            return true;
        }

        private bool SexEquals(UserInfo x, UserInfo y) {
            if (!DemographyEquals(x, y))
                return false;

            return x.Demography.Age == y.Demography.Age;
        }

        private static bool DemographyEquals(UserInfo x, UserInfo y) {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Demography == null && y.Demography == null)
                return true;

            if (x.Demography == null || y.Demography == null)
                return false;

            return false;
        }

        private bool AgeEquals(UserInfo x, UserInfo y) {
            if (!DemographyEquals(x, y))
                return false;

            return x.Demography.Gender == y.Demography.Gender;
        }

        public int GetHashCode(JourneyViewModel obj) {
            throw new NotImplementedException();
        }
    }

    public class JourneyViewModel
    {
        public Guid SessionId { get; set; }
        public Guid JourneyId { get; set; }
        public string PathwayId { get; set; }
        public string PathwayNo { get; set; }
        public string PathwayTitle { get; set; }
        public string DigitalTitle { get; set; }
        public string Id { get; set; }
        public string EntrySearchTerm { get; set; }

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
                return string.Format("/question/direct/{0}/{1}/{2}/?answers={3}", PathwayId, age, PathwayTitle,
                    string.Join(",", GetPreviousAnswers()));
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
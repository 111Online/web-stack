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
            QuestionPrimer,
            FirstQuestion,
            Question,
            InlineCareAdvice,
            PostcodeFirst,
            Outcome,
            ServiceDetails,
            ServiceList,
            PersonalDetails,
            TelphoneNumber,
            Confirmation,
            DuplicateBooking,
            BookingFailure,
            BookingUnavailable,
            Error,
            Call999Callback,
            AccidentAndEmergencyCallback,
            CheckAnswer,
            QuestionInformation,
            OutOfArea,
            RepeatPrescriptionRecommendedService,
            RepeatPrescriptionNotOffered,
            RepeatPrescriptionOtherServices,
            RepeatPrescriptionPreamble,
            RepeatPrescriptionReferralExplanation,
            PersonalDetailsCurrentAddress,
            PersonalDetailsCheckAtHome,
            PersonalDetailsConfirmDetails,
            PersonalDetailsUnknownAddress,
            PersonalDetailsManualAddress,
            PersonalDetailsChangeCurrentAddress,
            PersonalDetailsChangeCurrentPostcode,
            PersonalDetailsChangeHomePostcode,
            Explainer,
            GuidedSelection
        }

        public PageDataViewModel()
        {
            Page = PageType.ModuleZero;
        }
        public PageDataViewModel(PageType page, string campaign, string source)
        {
            Page = page;
            Campaign = campaign;
            Source = source;
        }

        public PageDataViewModel(PageType page, JourneyViewModel journey)
        {
            Page = page;
            Campaign = journey.Campaign;
            Source = journey.Source;
            if (journey.UserInfo != null && journey.UserInfo.Demography != null)
            {
                Gender = journey.UserInfo.Demography.Gender;
                Age = journey.UserInfo.Demography.Age.ToString();
            }
            SearchString = journey.EntrySearchTerm;
            QuestionId = journey.Id;
            TxNumber = !string.IsNullOrEmpty(journey.QuestionNo) && journey.QuestionNo.ToLower().StartsWith("tx") ? journey.QuestionNo : null;
            StartingPathwayNo = journey.PathwayNo;
            StartingPathwayTitle = !string.IsNullOrEmpty(journey.DigitalTitle) ? journey.DigitalTitle : journey.PathwayTitle;
            DxCode = !string.IsNullOrEmpty(journey.Id) && journey.Id.ToLower().StartsWith("dx") ? journey.Id : null;

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

        public bool IsCoronaJourney
        {
            get { return this.StartingPathwayNo == "PW1851"; }
        }

        public bool IsSmsJourney
        {
            get { return this.StartingPathwayNo == "PC111"; }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

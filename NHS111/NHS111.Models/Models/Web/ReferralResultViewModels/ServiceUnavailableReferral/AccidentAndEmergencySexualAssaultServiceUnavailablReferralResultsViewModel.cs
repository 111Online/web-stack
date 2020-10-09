namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencySexualAssaultServiceUnavailablReferralResultsViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string ViewName
        {
            get { return "Confirmation/SexualAssault/ServiceUnavailable"; }
        }
        public override string PageTitle { get { return "Sorry, this service has now closed"; } }

        public string Type { get; set; }

        public AccidentAndEmergencySexualAssaultServiceUnavailablReferralResultsViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencySexualAssaultServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}

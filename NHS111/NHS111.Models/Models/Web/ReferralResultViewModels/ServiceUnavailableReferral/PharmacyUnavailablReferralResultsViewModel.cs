namespace NHS111.Models.Models.Web
{
    public class PharmacyUnavailablReferralResultsViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string ViewName
        {
            get { return "Confirmation/Pharmacy/ServiceUnavailable"; }
        }
        public override string PageTitle { get { return "Sorry, this service has now closed"; } }

        public string Type { get; set; }

        public PharmacyUnavailablReferralResultsViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new PharmacyUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}

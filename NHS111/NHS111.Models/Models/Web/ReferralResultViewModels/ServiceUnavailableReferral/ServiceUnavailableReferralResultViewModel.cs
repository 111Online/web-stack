namespace NHS111.Models.Models.Web
{
    public class ServiceUnavailableReferralResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return "ServiceBookingUnavailable"; } }
        public override string PartialViewName { get { return "_ServiceUnavailable"; } }

        public ServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
            : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new ServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}

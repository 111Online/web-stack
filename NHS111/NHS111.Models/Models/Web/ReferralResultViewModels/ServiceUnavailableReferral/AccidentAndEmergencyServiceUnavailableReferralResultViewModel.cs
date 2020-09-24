namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencyServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_AccidentAndEmergencyServiceUnavailableReferral"; } }

        public AccidentAndEmergencyServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
            : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}

namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencySexualAssaultReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/SexualAssault/ServiceBookingFailure"; } }
        public override string PageTitle{ get { return "Sorry, there's a problem with the service"; }}

        public AccidentAndEmergencySexualAssaultReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencySexualAssaultReferralFailureAnalyticsDataLayer(this);
        }
    }
}

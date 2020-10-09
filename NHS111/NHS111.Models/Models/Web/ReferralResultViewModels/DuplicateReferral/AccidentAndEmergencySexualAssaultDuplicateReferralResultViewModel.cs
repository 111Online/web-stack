namespace NHS111.Models.Models.Web
{

    public class AccidentAndEmergencySexualAssaultDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithdetails/DuplicateReferral"; } }
        public override string PageTitle { get { return "Your call is confirmed"; } }
        public override string PartialViewName { get { return "DuplicateReferral"; } }

        public AccidentAndEmergencySexualAssaultDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencySexualAssaultDuplicateReferralAnalyticsDataLayer(this);
        }
    }
}

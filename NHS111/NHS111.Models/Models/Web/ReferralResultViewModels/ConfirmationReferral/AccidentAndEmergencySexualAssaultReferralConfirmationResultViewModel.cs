namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencySexualAssaultReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/SexualAssault/Confirmation"); } }
        public override string PageTitle{ get { return "Your call is confirmed"; } }

        public AccidentAndEmergencySexualAssaultReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencySexualAssaultReferralConfirmationAnalyticsDataLayer(this);
        }
    }
}

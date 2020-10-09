namespace NHS111.Models.Models.Web
{
    public class ClinicianDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/DuplicateReferral"; } }

        public ClinicianDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ClinicianDuplicateReferralAnalyticsDataLayer(this);
        }
    }
}

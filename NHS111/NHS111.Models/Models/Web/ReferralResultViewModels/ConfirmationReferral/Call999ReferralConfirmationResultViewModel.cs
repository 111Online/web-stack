namespace NHS111.Models.Models.Web
{
    public class Call999ReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/defaultWithDetails/Confirmation"); } }

        public Call999ReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
        : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Call999ReferralConfirmationAnalyticsDataLayer(this);
        }
    }
}

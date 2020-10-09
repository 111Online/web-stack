using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web
{
    public abstract class ReferralResultViewModel : BaseViewModel
    {
        public abstract string ViewName { get; }
        public ITKConfirmationViewModel ItkConfirmationModel { get; set; }
        public PersonalDetailViewModel OutcomeModel;
        public abstract string PartialViewName { get; }

        protected ReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
        {
            ItkConfirmationModel = itkConfirmationViewModel;
            OutcomeModel = itkConfirmationViewModel;
        }


        protected ReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
        {
            ItkConfirmationModel = new ITKConfirmationViewModel();
            OutcomeModel = outcomeViewModel;
        }
        protected string ResolveConfirmationViewByOutcome(OutcomeViewModel outcomeModel)
        {
            //todo:this needs a rethink with a combination of service type / outcome to route to correct page

            if (ShouldOverrideDefault(outcomeModel))
            {
                return outcomeModel.OutcomeGroup.Id;
            }

            return "default";
        }

        private static bool ShouldOverrideDefault(OutcomeViewModel outcomeModel)
        {
            return outcomeModel != null
                   && outcomeModel.OutcomeGroup != null
                   && (outcomeModel.OutcomeGroup.Equals(OutcomeGroup.Isolate111)
                        || outcomeModel.OutcomeGroup.Equals(OutcomeGroup.ItkPrimaryCareNer));
        }
    }
}

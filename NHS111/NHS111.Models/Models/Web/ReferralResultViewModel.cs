
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web {

    public abstract class BaseViewModel {
        public abstract string PageTitle { get; }
        public GoogleAnalyticsDataLayerContainer GoogleAnalyticsDataLayer { get; set; }
    }



    public abstract class ReferralResultViewModel : BaseViewModel
    {
        public abstract string ViewName { get; }
        public ITKConfirmationViewModel ItkConfirmationModel { get; set; }
        public OutcomeViewModel OutcomeModel;
        public abstract string PartialViewName { get; }

        protected ReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
        {
            ItkConfirmationModel = itkConfirmationViewModel;
            OutcomeModel = itkConfirmationViewModel;
        }


        protected ReferralResultViewModel(OutcomeViewModel outcomeViewModel)
        {
            OutcomeModel = outcomeViewModel;
        }

        protected string ResolveConfirmationViewByOutcome(OutcomeViewModel outcomeModel)
        {
            //todo:this needs a rethink with a combination of service type / outcome to route to correct page

            if (outcomeModel != null 
                && outcomeModel.OutcomeGroup != null
                && outcomeModel.OutcomeGroup.Equals(Domain.OutcomeGroup.RepeatPrescription))
            {
                return outcomeModel.OutcomeGroup.Id;
            }

            return "default";
        }
    }


    public class ReferralConfirmationResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Referral Confirmed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/Confirmation", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }
        public override string PartialViewName { get { return "_ReferralConfirmation"; } }

        public ReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new ReferralConfirmationResultGoogleAnalyticsDataLayer(this);
        }
    }


    public class Call999ReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel {
        public override string PartialViewName { get { return "_Call999ReferralConfirmation"; } }

            public Call999ReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new Call999ReferralConfirmationGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel {
        public override string PartialViewName { get { return "_AccidentAndEmergencyReferralConfirmation"; } }

        public AccidentAndEmergencyReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer(this);
        }
    }

    public class ReferralFailureResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceBookingFailure", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }
        public override string PartialViewName { get { return "_ReferralFailure"; } }

        public ReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new ReferralFailureResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralFailureResultViewModel
        : ReferralFailureResultViewModel {
        public override string PartialViewName { get { return "_AccidentAndEmergencyReferralFailure"; } }

        public AccidentAndEmergencyReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ReferralFailureResultViewModel
        : ReferralFailureResultViewModel {
        public override string PartialViewName { get { return "_Call999ReferralFailure"; } }

        public Call999ReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new Call999ReferralFailureGoogleAnalyticsDataLayer(this);
        }
    }

    public class DuplicateReferralResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - duplicate request for callback"; } }
        public override string ViewName { get { return "DuplicateBookingFailure"; } }
        public override string PartialViewName { get { return "_DuplicateReferral"; } }

        public DuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new DuplicateReferralResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel {
        public override string PartialViewName { get { return "_AccidentAndEmergencyDuplicateReferral"; } }

        public AccidentAndEmergencyDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999DuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel {
        public override string PartialViewName { get { return "_Call999DuplicateReferral"; } }

        public Call999DuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel) {
            GoogleAnalyticsDataLayer = new Call999DuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class EmergencyPrescriptionDuplicateReferralResultViewModel : DuplicateReferralResultViewModel
    {
        public override string PartialViewName { get { return "DuplicateReferral"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/DuplicateReferral", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }

        public EmergencyPrescriptionDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel): base(itkConfirmationViewModel)
        {
            GoogleAnalyticsDataLayer = new EmergencyPrescriptionDuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class ServiceUnavailableReferralResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return "ServiceBookingUnavailable"; } }
        public override string PartialViewName { get { return "_ServiceUnavailable"; } }

        public ServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeViewModel)
            : base(outcomeViewModel) {
            GoogleAnalyticsDataLayer = new ServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_AccidentAndEmergencyServiceUnavailableReferral"; } }

        public AccidentAndEmergencyServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeViewModel)
            : base(outcomeViewModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_Call999ServiceUnavailableReferral"; } }

        public Call999ServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeViewModel)
            : base(outcomeViewModel) {
            GoogleAnalyticsDataLayer = new Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class EmergencyPrescriptionServiceUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "ServiceUnavailable"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceUnavailable", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }

        public EmergencyPrescriptionServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            GoogleAnalyticsDataLayer = new EmergencyPrescriptionServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }
}
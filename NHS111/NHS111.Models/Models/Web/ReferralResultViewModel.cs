
using NHS111.Models.Models.Web;

namespace NHS111.Models.Models.Web {

    public abstract class BaseViewModel {
        public abstract string PageTitle { get; }
        public GoogleAnalyticsDataLayerContainer GoogleAnalyticsDataLayer { get; set; }
    }

    public abstract class ReferralResultViewModel
        : BaseViewModel {
        public abstract string ViewName { get; }
        public OutcomeViewModel OutcomeModel { get; set; }
        public abstract string PartialViewName { get; }

        protected ReferralResultViewModel(OutcomeViewModel outcomeModel) {
            OutcomeModel = outcomeModel;
        }

        protected  string ResolveConfirmationViewByOutcome(OutcomeViewModel outcomeModel)
        {
            if (outcomeModel.OutcomeGroup.Equals(Domain.OutcomeGroup.RepeatPrescription))
                return outcomeModel.OutcomeGroup.Id;
            return "default";
        }
    }

    public class ReferralConfirmationResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle { get { return "Referral Confirmed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/Confirmation", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }
        public override string PartialViewName { get { return "_ReferralConfirmation"; } }

        public ReferralConfirmationResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new ReferralConfirmationResultGoogleAnalyticsDataLayer(this);
        }
    }


    public class Call999ReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel {
        public override string PartialViewName { get { return "_Call999ReferralConfirmation"; } }

            public Call999ReferralConfirmationResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999ReferralConfirmationGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel {
        public override string PartialViewName { get { return "_AccidentAndEmergencyReferralConfirmation"; } }

        public AccidentAndEmergencyReferralConfirmationResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer(this);
        }
    }

    public class ReferralFailureResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceBookingFailure", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }
        public override string PartialViewName { get { return "_ReferralFailure"; } }

        public ReferralFailureResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new ReferralFailureResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralFailureResultViewModel
        : ReferralFailureResultViewModel {
        public override string PartialViewName { get { return "_AccidentAndEmergencyReferralFailure"; } }

        public AccidentAndEmergencyReferralFailureResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ReferralFailureResultViewModel
        : ReferralFailureResultViewModel {
        public override string PartialViewName { get { return "_Call999ReferralFailure"; } }

        public Call999ReferralFailureResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999ReferralFailureGoogleAnalyticsDataLayer(this);
        }
    }

    public class DuplicateReferralResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle { get { return "Call NHS 111 - duplicate request for callback"; } }
        public override string ViewName { get { return "DuplicateBookingFailure"; } }
        public override string PartialViewName { get { return "_DuplicateReferral"; } }

        public DuplicateReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new DuplicateReferralResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel {
        public override string PartialViewName { get { return "_AccidentAndEmergencyDuplicateReferral"; } }

        public AccidentAndEmergencyDuplicateReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999DuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel {
        public override string PartialViewName { get { return "_Call999DuplicateReferral"; } }

        public Call999DuplicateReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999DuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class ServiceUnavailableReferralResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return "ServiceBookingUnavailable"; } }
        public override string PartialViewName { get { return "_ServiceUnavailable"; } }

        public ServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new ServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_AccidentAndEmergencyServiceUnavailableReferral"; } }

        public AccidentAndEmergencyServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_Call999ServiceUnavailableReferral"; } }

        public Call999ServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }


}
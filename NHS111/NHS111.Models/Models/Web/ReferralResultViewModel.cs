
namespace NHS111.Models.Models.Web {
    using System.Collections.Generic;
    using System.Web;

    public abstract class GoogleAnalyticsDataLayerContainer
        : Dictionary<string, string> {

        public static string VirtualPageUrlKey = "virtualPageUrl";
        public static string VirtualPageTitleKey = "virtualPageTitle";
    }

    public abstract class ReferralResultGoogleAnalyticsDataLayer
        : GoogleAnalyticsDataLayerContainer {

        protected ReferralResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel) {
            this[VirtualPageUrlKey] = FormatUrl(viewModel);
            this[VirtualPageTitleKey] = VirtualPageTitle;
        }

        protected abstract string VirtualPageTitle { get; }
        protected abstract string VirtualUrlPageName { get; }

        private string FormatUrl(ReferralResultViewModel viewModel) {
            var pathwayNo = HttpUtility.UrlEncode(viewModel?.OutcomeModel?.PathwayNo);
            var outcomeGroup = HttpUtility.UrlEncode(viewModel?.OutcomeModel?.OutcomeGroup?.Text);
            var dxCode = HttpUtility.UrlEncode(viewModel?.OutcomeModel?.Id);
            var selectedServiceId = HttpUtility.UrlEncode(viewModel?.OutcomeModel?.SelectedServiceId);
            var selectedServiceName = HttpUtility.UrlEncode(viewModel?.OutcomeModel?.SelectedService?.Name);
            return string.Format("/outcome/{0}/{1}/{2}/itk/{3}/{4}/{5}/", pathwayNo, outcomeGroup, dxCode, VirtualUrlPageName, selectedServiceId, selectedServiceName);
        }
    }

    public class ReferralConfirmationResultGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "ITK Confirmation - Success";
        protected override string VirtualUrlPageName => "confirmation";

        public ReferralConfirmationResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ReferralConfirmationGoogleAnalyticsDataLayer
        : ReferralConfirmationResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "999 ITK Confirmation - Success";

        public Call999ReferralConfirmationGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer
        : ReferralConfirmationResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "A&E ITK Confirmation - Success";

        public AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ReferralFailureResultGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "ITK Confirmation - Failure";
        protected override string VirtualUrlPageName => "failure";

        public ReferralFailureResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer
        : ReferralFailureResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "A&E ITK Confirmation - Failure";

        public AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ReferralFailureGoogleAnalyticsDataLayer
        : ReferralFailureResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "999 ITK Confirmation - Failure";

        public Call999ReferralFailureGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class DuplicateReferralResultGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "ITK Confirmation - Duplicate Booking";
        protected override string VirtualUrlPageName => "failure";

        public DuplicateReferralResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer
        : DuplicateReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "A&E ITK Confirmation - Duplicate Booking";

        public AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999DuplicateReferralGoogleAnalyticsDataLayer
        : DuplicateReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "999 ITK Confirmation - Duplicate Booking";

        public Call999DuplicateReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ServiceUnavailableReferralGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "ITK Confirmation - Unavailable";
        protected override string VirtualUrlPageName => "serviceUnavailable";

        public ServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer
        : ServiceUnavailableReferralGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "A&E ITK Confirmation - Unavailable";

        public AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer
        : ServiceUnavailableReferralGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle => "999 ITK Confirmation - Unavailable";

        public Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

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
    }

    public class ReferralConfirmationResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle => "Referral Confirmed";
        public override string ViewName => "Confirmation";
        public override string PartialViewName => "_ReferralConfirmation";

        public ReferralConfirmationResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new ReferralConfirmationResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel {
        public override string PartialViewName => "_Call999ReferralConfirmation";

        public Call999ReferralConfirmationResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999ReferralConfirmationGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel {
        public override string PartialViewName => "_AccidentAndEmergencyReferralConfirmation";

        public AccidentAndEmergencyReferralConfirmationResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer(this);
        }
    }

    public class ReferralFailureResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle => "Call NHS 111 - request for callback not completed";
        public override string ViewName => "ServiceBookingFailure";
        public override string PartialViewName => "_ReferralFailure";

        public ReferralFailureResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new ReferralFailureResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralFailureResultViewModel
        : ReferralFailureResultViewModel {
        public override string PartialViewName => "_AccidentAndEmergencyReferralFailure";

        public AccidentAndEmergencyReferralFailureResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ReferralFailureResultViewModel
        : ReferralFailureResultViewModel {
        public override string PartialViewName => "_Call999ReferralFailure";

        public Call999ReferralFailureResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999ReferralFailureGoogleAnalyticsDataLayer(this);
        }
    }

    public class DuplicateReferralResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle => "Call NHS 111 - duplicate request for callback";
        public override string ViewName => "DuplicateBookingFailure";
        public override string PartialViewName => "_DuplicateReferral";

        public DuplicateReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new DuplicateReferralResultGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel {
        public override string PartialViewName => "_AccidentAndEmergencyDuplicateReferral";

        public AccidentAndEmergencyDuplicateReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999DuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel {
        public override string PartialViewName => "_Call999DuplicateReferral";

        public Call999DuplicateReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999DuplicateReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class ServiceUnavailableReferralResultViewModel
        : ReferralResultViewModel {
        public override string PageTitle => "Call NHS 111 - request for callback not completed";
        public override string ViewName => "ServiceBookingUnavailable";
        public override string PartialViewName => "_ServiceUnavailable";

        public ServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new ServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName => "_AccidentAndEmergencyServiceUnavailableReferral";

        public AccidentAndEmergencyServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }

    public class Call999ServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName => "_Call999ServiceUnavailableReferral";

        public Call999ServiceUnavailableReferralResultViewModel(OutcomeViewModel outcomeModel)
            : base(outcomeModel) {
            GoogleAnalyticsDataLayer = new Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer(this);
        }
    }


}
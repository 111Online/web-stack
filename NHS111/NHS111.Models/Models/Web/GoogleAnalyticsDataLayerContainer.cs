
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
            return string.Format("/outcome/{0}/{1}/{2}/itk/{3}/{4}/{5}/", pathwayNo, outcomeGroup, dxCode,
                VirtualUrlPageName, selectedServiceId, selectedServiceName);
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
}
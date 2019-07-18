
namespace NHS111.Models.Models.Web {
    using System.Collections.Generic;
    using System.Web;
    using FromExternalServices;
    using Mappers.WebMappings;

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
            if (viewModel == null || viewModel.OutcomeModel == null ||
                viewModel.OutcomeModel.OutcomeGroup == null || viewModel.OutcomeModel.SelectedService == null)
                return "";

            var pathwayNo = HttpUtility.UrlEncode(viewModel.OutcomeModel.PathwayNo);
            var outcomeGroup = HttpUtility.UrlEncode(viewModel.OutcomeModel.OutcomeGroup.Text);
            var dxCode = HttpUtility.UrlEncode(viewModel.OutcomeModel.Id);
            var selectedServiceId = HttpUtility.UrlEncode(viewModel.OutcomeModel.SelectedServiceId);
            var selectedServiceName = HttpUtility.UrlEncode(viewModel.OutcomeModel.SelectedService.Name);
            
            var url = string.Format("/outcome/{0}/{1}/{2}/itk/{3}/{4}/{5}/", pathwayNo, outcomeGroup, dxCode,
                VirtualUrlPageName, selectedServiceId, selectedServiceName);
            if (viewModel.OutcomeModel.HasAcceptedCallbackOffer.HasValue && viewModel.OutcomeModel.HasAcceptedCallbackOffer.Value)
                url += FromOutcomeViewModelToDosViewModel.DispositionResolver.Remap(viewModel.OutcomeModel.Id) + "/";
            return url;
        }
    }

    public class ReferralConfirmationResultGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Success"; } }

        protected override string VirtualUrlPageName { get { return "confirmation"; } }

        public ReferralConfirmationResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ReferralConfirmationGoogleAnalyticsDataLayer
        : ReferralConfirmationResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Success"; } }

        public Call999ReferralConfirmationGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer
        : ReferralConfirmationResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Success"; } }

        public AccidentAndEmergencyReferralConfirmationGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class EmergencyPrescriptionReferralConfirmationGoogleAnalyticsDataLayer
        : ReferralConfirmationResultGoogleAnalyticsDataLayer
    {

        public EmergencyPrescriptionReferralConfirmationGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) {
            this[VirtualPageTitleKey] = string.Format("Emergency Prescription {0} ITK Confirmation - Success", viewModel.ItkConfirmationModel.SelectedService.OnlineDOSServiceType.Id);
        }

        private readonly OnlineDOSServiceType _serviceType;
    }

    public class ReferralFailureResultGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Failure"; } }

        protected override string VirtualUrlPageName { get { return "failure"; } }

        public ReferralFailureResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer
        : ReferralFailureResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Failure"; } }

        public AccidentAndEmergencyReferralFailureGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ReferralFailureGoogleAnalyticsDataLayer
        : ReferralFailureResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Failure"; } }

        public Call999ReferralFailureGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class DuplicateReferralResultGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Duplicate Booking"; } }

        protected override string VirtualUrlPageName { get { return "failure"; } }

        public DuplicateReferralResultGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer
        : DuplicateReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Duplicate Booking"; } }

        public AccidentAndEmergencyDuplicateReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999DuplicateReferralGoogleAnalyticsDataLayer
        : DuplicateReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Duplicate Booking"; } }

        public Call999DuplicateReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class EmergencyPrescriptionDuplicateReferralGoogleAnalyticsDataLayer : DuplicateReferralResultGoogleAnalyticsDataLayer
    {
        protected override string VirtualPageTitle { get { return "Emergency prescription ITK Confirmation - Duplicate Booking"; } }

        public EmergencyPrescriptionDuplicateReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ServiceUnavailableReferralGoogleAnalyticsDataLayer
        : ReferralResultGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Unavailable"; } }

        protected override string VirtualUrlPageName { get { return "serviceUnavailable"; } }

        public ServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer
        : ServiceUnavailableReferralGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Unavailable"; } }

        public AccidentAndEmergencyServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer
        : ServiceUnavailableReferralGoogleAnalyticsDataLayer {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Unavailable"; } }

        public Call999ServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class EmergencyPrescriptionServiceUnavailableReferralGoogleAnalyticsDataLayer : ServiceUnavailableReferralGoogleAnalyticsDataLayer
    {
        protected override string VirtualPageTitle { get { return "Emergency prescription ITK Confirmation - Unavailable"; } }

        public EmergencyPrescriptionServiceUnavailableReferralGoogleAnalyticsDataLayer(ReferralResultViewModel viewModel) : base(viewModel) { }
    }
}
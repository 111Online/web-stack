
namespace NHS111.Models.Models.Web
{
    using FromExternalServices;
    using Mappers.WebMappings;
    using System.Collections.Generic;
    using System.Web;

    public abstract class AnalyticsDataLayerContainer
        : Dictionary<string, string>
    {

        public static string VirtualPageUrlKey = "virtualPageUrl";
        public static string VirtualPageTitleKey = "virtualPageTitle";
    }

    public abstract class ReferralResultAnalyticsDataLayer
        : AnalyticsDataLayerContainer
    {

        protected ReferralResultAnalyticsDataLayer(ReferralResultViewModel viewModel)
        {
            this[VirtualPageUrlKey] = FormatUrl(viewModel);
            this[VirtualPageTitleKey] = VirtualPageTitle;
        }

        protected abstract string VirtualPageTitle { get; }
        protected abstract string VirtualUrlPageName { get; }

        private string FormatUrl(ReferralResultViewModel viewModel)
        {
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

    public class ReferralConfirmationResultAnalyticsDataLayer
        : ReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Success"; } }

        protected override string VirtualUrlPageName { get { return "confirmation"; } }

        public ReferralConfirmationResultAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ClinicianCallbackReferralConfirmationAnalyticsDataLayer
        : ReferralConfirmationResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "Clinical callback confirmation - Success"; } }

        public ClinicianCallbackReferralConfirmationAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ReferralConfirmationAnalyticsDataLayer
        : ReferralConfirmationResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Success"; } }

        public Call999ReferralConfirmationAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyReferralConfirmationAnalyticsDataLayer
        : ReferralConfirmationResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Success"; } }

        public AccidentAndEmergencyReferralConfirmationAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }


    public class CoronaReferralConfirmationAnalyticsDataLayer
        : ReferralConfirmationResultAnalyticsDataLayer
    {

        public CoronaReferralConfirmationAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel)
        {
            this[VirtualPageTitleKey] = "Corona test kit booking - Success";
        }
    }


    public class ServiceFirstReferralConfirmationAnalyticsDataLayer
        : ReferralConfirmationResultAnalyticsDataLayer
    {

        public ServiceFirstReferralConfirmationAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel)
        {
            this[VirtualPageTitleKey] = string.Format("Service First {0} ITK Confirmation - Success", viewModel.ItkConfirmationModel.SelectedService.OnlineDOSServiceType.Id);
        }
    }

    public class Coronavirus111CallbackReferralConfirmationAnalyticsDataLayer
        : ReferralConfirmationResultAnalyticsDataLayer
    {

        public Coronavirus111CallbackReferralConfirmationAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel)
        {
            this[VirtualPageTitleKey] = string.Format("Coronavirus 111 Callback {0} ITK Confirmation - Success", viewModel.ItkConfirmationModel.SelectedService.OnlineDOSServiceType.Id);
        }
    }

    public class ReferralFailureResultAnalyticsDataLayer
        : ReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Failure"; } }

        protected override string VirtualUrlPageName { get { return "failure"; } }

        public ReferralFailureResultAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyReferralFailureAnalyticsDataLayer
        : ReferralFailureResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Failure"; } }

        public AccidentAndEmergencyReferralFailureAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ClinicianReferralFailureAnalyticsDataLayer
        : ReferralFailureResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "Clinician ITK Confirmation - Failure"; } }

        public ClinicianReferralFailureAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }


    public class Coronavirus111CallbackReferralFailureAnalyticsDataLayer
        : ReferralFailureResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "Coronavirus 111 Callback - Failure"; } }

        public Coronavirus111CallbackReferralFailureAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }


    public class CoronaReferralFailureAnalyticsDataLayer
        : ReferralFailureResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "Corona Test kit - Failure"; } }

        public CoronaReferralFailureAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ReferralFailureAnalyticsDataLayer
        : ReferralFailureResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Failure"; } }

        public Call999ReferralFailureAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ServiceFirstReferralFailureAnalyticsDataLayer
        : ReferralFailureResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "Service first ITK Confirmation - Failure"; } }

        public ServiceFirstReferralFailureAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class DuplicateReferralResultAnalyticsDataLayer
        : ReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Duplicate Booking"; } }

        protected override string VirtualUrlPageName { get { return "failure"; } }

        public DuplicateReferralResultAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyDuplicateReferralAnalyticsDataLayer
        : DuplicateReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Duplicate Booking"; } }

        public AccidentAndEmergencyDuplicateReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    
    public class ClinicianDuplicateReferralAnalyticsDataLayer
        : DuplicateReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "Clinician ITK Confirmation - Duplicate Booking"; } }

        public ClinicianDuplicateReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999DuplicateReferralAnalyticsDataLayer
        : DuplicateReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Duplicate Booking"; } }

        public Call999DuplicateReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ServiceFirstDuplicateReferralAnalyticsDataLayer : DuplicateReferralResultAnalyticsDataLayer
    {
        protected override string VirtualPageTitle { get { return "Service First ITK Confirmation - Duplicate Booking"; } }

        public ServiceFirstDuplicateReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ServiceUnavailableReferralAnalyticsDataLayer
        : ReferralResultAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "ITK Confirmation - Unavailable"; } }

        protected override string VirtualUrlPageName { get { return "serviceUnavailable"; } }

        public ServiceUnavailableReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralAnalyticsDataLayer
        : ServiceUnavailableReferralAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "A&E ITK Confirmation - Unavailable"; } }

        public AccidentAndEmergencyServiceUnavailableReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class Call999ServiceUnavailableReferralAnalyticsDataLayer
        : ServiceUnavailableReferralAnalyticsDataLayer
    {

        protected override string VirtualPageTitle { get { return "999 ITK Confirmation - Unavailable"; } }

        public Call999ServiceUnavailableReferralAnalyticsDataLayer(ReferralResultViewModel viewModel)
            : base(viewModel) { }
    }

    public class ServiceFirstServiceUnavailableReferralAnalyticsDataLayer : ServiceUnavailableReferralAnalyticsDataLayer
    {
        protected override string VirtualPageTitle { get { return "Service First ITK Confirmation - Unavailable"; } }

        public ServiceFirstServiceUnavailableReferralAnalyticsDataLayer(ReferralResultViewModel viewModel) : base(viewModel) { }
    }
    public class TestKitServiceUnavailableReferralAnalyticsDataLayer : ServiceUnavailableReferralAnalyticsDataLayer
    {
        protected override string VirtualPageTitle { get { return "Test Kit Confirmation - Unavailable"; } }

        public TestKitServiceUnavailableReferralAnalyticsDataLayer(ReferralResultViewModel viewModel) : base(viewModel) { }
    }

    public class Coronavirus111CallbackServiceUnavailableReferralAnalyticsDataLayer : ServiceUnavailableReferralAnalyticsDataLayer
    {
        protected override string VirtualPageTitle { get { return "Coronavirus 111 Callback - Unavailable"; } }

        public Coronavirus111CallbackServiceUnavailableReferralAnalyticsDataLayer(ReferralResultViewModel viewModel) : base(viewModel) { }
    }
}
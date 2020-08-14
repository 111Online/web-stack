using NHS111.Models.Models.Domain;
using StructureMap.Query;

namespace NHS111.Models.Models.Web {

    public abstract class BaseViewModel
    {
        public abstract string PageTitle { get; }
        public AnalyticsDataLayerContainer AnalyticsDataLayer { get; set; }
    }

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

    public class ReferralConfirmationResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Referral Confirmed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/Confirmation", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }
        public override string PartialViewName { get { return "_ReferralConfirmation"; } }

        public ReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ReferralConfirmationResultAnalyticsDataLayer(this);
        }
    }

    public class TemporaryReferralDuplicateReferralResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public TemporaryReferralDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) :
            base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new DuplicateReferralResultAnalyticsDataLayer(this);
        }
    }


    public class ClinicianCallbackReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/defaultWithDetails/Confirmation"); } }

        public ClinicianCallbackReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
        : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ClinicianCallbackReferralConfirmationAnalyticsDataLayer(this);
        }
    }

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

    public class AccidentAndEmergencyReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/defaultWithDetails/Confirmation"); } }

        public AccidentAndEmergencyReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyReferralConfirmationAnalyticsDataLayer(this);
        }
    }


    public class CoronaReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public CoronaReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new CoronaReferralFailureAnalyticsDataLayer(this);
        }
    }

    public class ServiceFirstReferralConfirmationResultsViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName 
        { 
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/Confirmation", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/Confirmation";
            }
        }

        public ServiceFirstReferralConfirmationResultsViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstReferralConfirmationAnalyticsDataLayer(this);
        }
    }

    public class Coronavirus111CallbackReferralConfirmationResultsViewModel
        : ReferralConfirmationResultViewModel
    {

        public Coronavirus111CallbackReferralConfirmationResultsViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Coronavirus111CallbackReferralConfirmationAnalyticsDataLayer(this);
        }
    }

    public class ReferralFailureResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceBookingFailure", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }
        public override string PartialViewName { get { return "_ReferralFailure"; } }

        public ReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ReferralFailureResultAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/ServiceBookingFailure"; } }

        public AccidentAndEmergencyReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyReferralFailureAnalyticsDataLayer(this);
        }
    }

    public class ClinicianReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/ServiceBookingFailure"; } }

        public ClinicianReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ClinicianReferralFailureAnalyticsDataLayer(this);
        }
    }


    public class Coronavirus111CallbackReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/default/ServiceBookingFailure"; } }

        public Coronavirus111CallbackReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Coronavirus111CallbackReferralFailureAnalyticsDataLayer(this);
        }
    }

    public class CoronaReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public CoronaReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new CoronaReferralFailureAnalyticsDataLayer(this);
        }
    }

    public class Call999ReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/ServiceBookingFailure"; } }

        public Call999ReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Call999ReferralFailureAnalyticsDataLayer(this);
        }
    }

    public class ServiceFirstReferralFailureResultViewModel : ReferralFailureResultViewModel
    {
        public override string ViewName
        {
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/ServiceBookingFailure", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/ServiceBookingFailure";
            }
        }

        public ServiceFirstReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstReferralFailureAnalyticsDataLayer(this);
        }
    }

    public class DuplicateReferralResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - duplicate request for callback"; } }
        public override string ViewName { get { return "DuplicateBookingFailure"; } }
        public override string PartialViewName { get { return "_DuplicateReferral"; } }

        public DuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new DuplicateReferralResultAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/DuplicateReferral"; } }

        public AccidentAndEmergencyDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyDuplicateReferralAnalyticsDataLayer(this);
        }
    }

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

    public class Call999DuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/DuplicateReferral"; } }

        public Call999DuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Call999DuplicateReferralAnalyticsDataLayer(this);
        }
    }

    //Temporarily removed until status of Dupe bug is known https://trello.com/c/5hqJVLDv
    public class TemporaryServiceFirstDuplicateReferralResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName
        {
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/Confirmation", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/Confirmation";
            }
        }

        public TemporaryServiceFirstDuplicateReferralResultViewModel(
            ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstDuplicateReferralAnalyticsDataLayer(this);
        }
    }

    public class ServiceFirstDuplicateReferralResultViewModel : DuplicateReferralResultViewModel
    {
        public override string ViewName
        {
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/DuplicateReferral", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/DuplicateReferral";
            }
        }
        public override string PartialViewName { get { return "DuplicateReferral"; } }

        public ServiceFirstDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstDuplicateReferralAnalyticsDataLayer(this);
        }
    }

    public class ServiceUnavailableReferralResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return "ServiceBookingUnavailable"; } }
        public override string PartialViewName { get { return "_ServiceUnavailable"; } }

        public ServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
            : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new ServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }

    public class AccidentAndEmergencyServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_AccidentAndEmergencyServiceUnavailableReferral"; } }

        public AccidentAndEmergencyServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
            : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }

    public class Call999ServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_Call999ServiceUnavailableReferral"; } }

        public Call999ServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
            : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new Call999ServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }

    public class ServiceFirstServiceUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_AccidentAndEmergencyServiceUnavailableReferral"; } }
        public override string ViewName
        {
            get
            {
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? "ServiceBookingUnavailable" : "Confirmation/Service_first/Emergency_Prescription/ServiceUnavailable";
            }
        }

        public ServiceFirstServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }

    public class TestKitServiceUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "ServiceUnavailable"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceUnavailable", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }

        public TestKitServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new TestKitServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }

    public class Coronavirus111CallbackUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "ServiceUnavailable"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceUnavailable", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }

        public Coronavirus111CallbackUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new Coronavirus111CallbackServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}

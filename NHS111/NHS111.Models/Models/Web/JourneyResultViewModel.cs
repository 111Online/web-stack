using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web
{
    public abstract class JourneyResultViewModel
    {
        public abstract string ViewName { get; }
        public JourneyViewModel JourneyModel { get; set; }
        public OutcomeViewModel OutcomeModel { get; set; }

        protected JourneyResultViewModel(JourneyViewModel journeyViewModel)
        {
            JourneyModel = journeyViewModel;
        }

        protected JourneyResultViewModel(OutcomeViewModel outcomeViewModel)
        {
            OutcomeModel = outcomeViewModel;
        }

        protected string ResolveViewByOutcomeResult(OutcomeResultViewModel outcomeResultViewModel)
        {
            var outcome = outcomeResultViewModel.OutcomeModel.OutcomeGroup;
            var subfolder = "";
            if (outcome.IsPharmacyGroup)
                subfolder = outcome.Id + "/";
            if (outcome.IsPrimaryCare)
                subfolder = "Primary_Care/";
            if (outcome.IsCoronaVirus)
                subfolder = "Corona/";
            if (outcome.IsSendSMS || outcome.IsVerifySMS)
                subfolder = "SMS/";

            if (OutcomeGroup.Isolate111.Equals(outcome))
            {
                if (outcomeResultViewModel.OutcomeModel.DosCheckCapacitySummaryResult.HasITKServices)
                    return "../Outcome/Corona/ITK_Clinician_call_back";
            }

            var viewFilePath = "../Outcome/" + subfolder;

            var model = outcomeResultViewModel.OutcomeModel;
            if (model.OutcomeGroup.IsUsingRecommendedService)
            {
                if (model.RecommendedService == null) return viewFilePath + "RecommendedServiceNotOffered";

                if (model.OutcomeGroup.RequiresOutcomePreamble(model.HasSeenPreamble))
                    viewFilePath += "Outcome_Preamble";
                else
                    viewFilePath += "RecommendedService";
            }
            else
                viewFilePath += model.OutcomeGroup.Id;

            if (outcomeResultViewModel.IsTestJourney)
                return "../Outcome/Call_999_CheckAnswer";

            if (model.Is999Callback)
                return "../Outcome/Call_999_Callback";

            if (model.OutcomeGroup.Equals(OutcomeGroup.AccidentAndEmergency) || model.OutcomeGroup.Equals(OutcomeGroup.MentalHealth))
            {

                if (!model.DosCheckCapacitySummaryResult.IsValidationRequery && model.DosCheckCapacitySummaryResult.HasITKServices && !model.HasAcceptedCallbackOffer.HasValue)
                    return "../Outcome/SP_Accident_and_emergency_callback";
            }

            return viewFilePath;
        }

        protected bool isNHSUKStyle()
        {
            if (JourneyModel != null) return JourneyModel.PathwayNo != null && JourneyModel.PathwayNo.Equals("PC111");
            return false;
        }
    }

    public class QuestionResultViewModel : JourneyResultViewModel
    {
        public override string ViewName
        {
            get
            {
                if (isNHSUKStyle()) return "../Question/Custom/NHSUKQuestion";
                return "../Question/Question";
            }
        }

        public QuestionResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class OutcomeResultViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return ResolveViewByOutcomeResult(this); } }

        public bool IsTestJourney { get; set; }

        public OutcomeResultViewModel(OutcomeViewModel outcomeViewModel, bool isTestJourneyResult) : base(outcomeViewModel)
        {
            IsTestJourney = isTestJourneyResult;
        }
    }

    public class PageResultViewModel : JourneyResultViewModel
    {
        private string _viewName;
        public override string ViewName { get { return _viewName; } }
        public PageResultViewModel(JourneyViewModel journeyViewModel, string viewName) : base(journeyViewModel)
        {
            _viewName = viewName;
        }
        public PageResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
            if (isNHSUKStyle()) _viewName = "../Question/Custom/NHSUKPage";
            _viewName = "../Question/Page";
        }
    }

    public class CareAdviceResultViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../Question/InlineCareAdvice"; } }

        public CareAdviceResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class PathwaySelectionJumpResultViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../Outcome/PathwaySelectionJump"; } }

        public PathwaySelectionJumpResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class DeadEndJumpResultViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../Outcome/DeadEndJump"; } }

        public DeadEndJumpResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class SearchJumpViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../Search/Search"; } }

        public SearchJumpViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class VerifyForSMSViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../RegisterForSMS/Verify_SMS"; } }

        public VerifyForSMSViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class NodeNotFoundViewModel : JourneyResultViewModel
    {

        public override string ViewName { get { return "../Question/Not_Found"; } }

        public NodeNotFoundViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel) { }
    }

    public class RegisterForSMSViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../RegisterForSMS/Send_SMS"; } }

        public RegisterForSMSViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }
}

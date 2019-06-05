using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            var viewFilePath = string.Format("../Outcome/{0}", outcome.IsPharmacyGroup ? outcome.Id + "/" : string.Empty);
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

            if (model.OutcomeGroup.Equals(OutcomeGroup.AccidentAndEmergency))
            {

                if (!model.DosCheckCapacitySummaryResult.IsValidationRequery && model.DosCheckCapacitySummaryResult.HasITKServices && !model.HasAcceptedCallbackOffer.HasValue)
                    return "../Outcome/SP_Accident_and_emergency_callback";
            }
            return viewFilePath;
        }
    }

    public class QuestionResultViewModel: JourneyResultViewModel
    {
        public override string ViewName { get { return "../Question/Question"; } }

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
        public override string ViewName { get { return "../Question/Page"; } }

        public PageResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
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
        public override string ViewName { get { return "../Question/PathwaySelectionJump"; } }

        public PathwaySelectionJumpResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }

    public class DeadEndJumpResultViewModel : JourneyResultViewModel
    {
        public override string ViewName { get { return "../Question/DeadEndJump"; } }

        public DeadEndJumpResultViewModel(JourneyViewModel journeyViewModel) : base(journeyViewModel)
        {
        }
    }
}

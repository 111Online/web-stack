using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;
using System;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Web.Helpers
{
    using Models.Models.Domain;
    using Newtonsoft.Json;
    using Presentation.Configuration;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class ViewRouter : IViewRouter
    {
        private readonly IAuditLogger _auditLogger;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly IJourneyViewModelEqualityComparer _journeyViewModelComparer;

        public ViewRouter(IAuditLogger auditLogger, IUserZoomDataBuilder userZoomDataBuilder, IJourneyViewModelEqualityComparer journeyViewModelComparer)
        {
            _auditLogger = auditLogger;
            _userZoomDataBuilder = userZoomDataBuilder;
            _journeyViewModelComparer = journeyViewModelComparer;
        }

        public string GetOutcomeViewPath(OutcomeViewModel model, ControllerContext context, string nextView)
        {
            var viewFilePath = string.Format("../PostcodeFirst/{0}/{1}", model.OutcomeGroup.Id, nextView);
            if (ViewExists(viewFilePath, context))
            {
                _userZoomDataBuilder.SetFieldsForOutcome(model, context.RequestContext);
                return viewFilePath;
            }
            throw new ArgumentOutOfRangeException(string.Format("Outcome group {0} for outcome {1} has no view configured", model.OutcomeGroup.ToString(), model.Id));
        }

        public string GetCallbackConfirmationViewName(OutcomeGroup outcomeGroup)
        {
            return outcomeGroup.Is999NonUrgent ? "Call_999_Callback_Confirmation" : "Confirmation";
        }

        public string GetCallbackFailureViewName(OutcomeGroup outcomeGroup)
        {
            return outcomeGroup.Is999NonUrgent ? "Call999_ServiceBookingFailure" : "ServiceBookingFailure";
        }
        public string GetServiceUnavailableViewName(OutcomeGroup outcomeGroup)
        {
            return outcomeGroup.Is999NonUrgent ? "Call999_ServiceBookingUnavailable" : "ServiceBookingUnavailable";
        }

        public string GetCallbackDuplicateViewName(OutcomeGroup outcomeGroup)
        {
            return outcomeGroup.Is999NonUrgent ? "Call999_DuplicateBookingFailure" : "DuplicateBookingFailure";
        }

        public JourneyResultViewModel Build(JourneyViewModel journeyViewModel, ControllerContext context)
        {
            if (journeyViewModel == null)
                return new QuestionResultViewModel(journeyViewModel);

            switch (journeyViewModel.NodeType)
            {
                case NodeType.Outcome:

                    if (journeyViewModel.OutcomeGroup.Id == "111_Search_Jump")
                    {
                        // This outcome is used to go from mid-pathway into search
                        return new SearchJumpViewModel(journeyViewModel);
                    }

                    if (journeyViewModel.OutcomeGroup.IsVerifySMS)
                    {
                        return new VerifyForSMSViewModel(journeyViewModel);
                    }

                    if (journeyViewModel.OutcomeGroup.IsSendSMS)
                    {
                        return new RegisterForSMSViewModel(journeyViewModel);
                    }

                    var outcomeViewModel = journeyViewModel as OutcomeViewModel;
                    var result = new OutcomeResultViewModel(outcomeViewModel, IsTestJourney(outcomeViewModel));
                    if (ViewExists(result.ViewName, context))
                    {
                        _userZoomDataBuilder.SetFieldsForOutcome(journeyViewModel, context.RequestContext);
                        return result;
                    }
                    throw new ArgumentOutOfRangeException(string.Format("Outcome group {0} for outcome {1} has no view configured", outcomeViewModel.OutcomeGroup.Id, outcomeViewModel.Id));
                case NodeType.DeadEndJump:
                    _userZoomDataBuilder.SetFieldsForOutcome(journeyViewModel, context.RequestContext);
                    return new DeadEndJumpResultViewModel(journeyViewModel);
                case NodeType.PathwaySelectionJump:
                    _userZoomDataBuilder.SetFieldsForOutcome(journeyViewModel, context.RequestContext);
                    return new PathwaySelectionJumpResultViewModel(journeyViewModel);
                case NodeType.CareAdvice:
                    _userZoomDataBuilder.SetFieldsForCareAdvice(journeyViewModel, context.RequestContext);
                    return new CareAdviceResultViewModel(journeyViewModel);
                case NodeType.Page:
                    if (journeyViewModel.Content != null && journeyViewModel.Content.StartsWith("!CustomView!"))
                        return new PageResultViewModel(journeyViewModel, String.Format("../Question/Custom/{0}", journeyViewModel.Content.Replace("!CustomView!", "")));

                    return new PageResultViewModel(journeyViewModel);
                case NodeType.Question:
                default:
                    _userZoomDataBuilder.SetFieldsForQuestion(journeyViewModel, context.RequestContext);
                    return new QuestionResultViewModel(journeyViewModel);
            }
        }

        private bool IsTestJourney(OutcomeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.TriggerQuestionNo)) //have we already seen the trigger question screen?
                return false;
            var testJourneys = ReadTestJourneys();

            foreach (var testJourney in testJourneys)
            {
                var result = JsonConvert.DeserializeObject<OutcomeViewModel>(testJourney.Json);
                if (_journeyViewModelComparer.Equals(model, result))
                {
                    model.TriggerQuestionNo = testJourney.TriggerQuestionNo;
                    model.TriggerQuestionAnswer = model.Journey.Steps
                        .First(a => a.QuestionNo == model.TriggerQuestionNo).Answer.Title;
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<TestJourneyElement> ReadTestJourneys()
        {
            var section = ConfigurationManager.GetSection("testJourneySection");
            if (!(section is TestJourneysConfigurationSection))
                return new List<TestJourneyElement>();

            return (section as TestJourneysConfigurationSection)
                .TestJourneys
                .Cast<TestJourneyElement>();
        }

        private bool ViewExists(string name, ControllerContext context)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(context, name, null);
            return (result.View != null);
        }
    }

    public interface IViewRouter
    {
        JourneyResultViewModel Build(JourneyViewModel model, ControllerContext context);
        string GetOutcomeViewPath(OutcomeViewModel model, ControllerContext context, string nextView);
        string GetCallbackFailureViewName(OutcomeGroup outcomeGroup);
        string GetCallbackDuplicateViewName(OutcomeGroup outcomeGroup);
        string GetCallbackConfirmationViewName(OutcomeGroup outcomeGroup);
        string GetServiceUnavailableViewName(OutcomeGroup outcomeGroup);
    }
}

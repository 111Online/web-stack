using System;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Helpers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Controllers;
    using Features;
    using Models.Models.Domain;
    using Newtonsoft.Json;
    using Presentation.Configuration;

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
                _userZoomDataBuilder.SetFieldsForOutcome(model);
                return viewFilePath;
            }
            throw new ArgumentOutOfRangeException(string.Format("Outcome group {0} for outcome {1} has no view configured", model.OutcomeGroup.ToString(), model.Id));
        }

        public string GetCallbackConfirmationViewName(OutcomeGroup outcomeGroup) {
            return outcomeGroup.Is999NonUrgent ? "Call_999_Callback_Confirmation" : "Confirmation";
        }

        public string GetCallbackFailureViewName(OutcomeGroup outcomeGroup) {
            return outcomeGroup.Is999NonUrgent ? "Call999_ServiceBookingFailure" : "ServiceBookingFailure";
        }
        public string GetServiceUnavailableViewName(OutcomeGroup outcomeGroup) {
            return outcomeGroup.Is999NonUrgent ? "Call999_ServiceBookingUnavailable" : "ServiceBookingUnavailable";
        }

        public string GetCallbackDuplicateViewName(OutcomeGroup outcomeGroup) {
            return outcomeGroup.Is999NonUrgent ? "Call999_DuplicateBookingFailure" : "DuplicateBookingFailure";
        }

        public string GetViewName(JourneyViewModel model, ControllerContext context)
        {
            if (model == null) return "../Question/Question";

            switch (model.NodeType)
            {
                case NodeType.Outcome:
                    var outcomeViewModel = model as OutcomeViewModel;
                    var viewFilePath = "../Outcome/";
                    if (OutcomeGroup.UsingRecommendedServiceJourney.Contains(model.OutcomeGroup))
                    {
                        if (outcomeViewModel.RecommendedService == null) viewFilePath += "NoResults"; //TODO: Build this page

                        if (OutcomeGroup.RequiresOutcomePreamble.Contains(model.OutcomeGroup))
                            viewFilePath += "Outcome_Preamble";
                        else
                            viewFilePath += "RecommendedService"; 
                    }
                    else
                        viewFilePath += model.OutcomeGroup.Id;
                    //if (model.OutcomeGroup.IsPostcodeFirst())
                    //{
                    //    model.UserInfo.CurrentAddress.IsPostcodeFirst = true;
                    //    _auditLogger.LogEventData(model, "Postcode first journey started");

                    //    viewFilePath = "../PostcodeFirst/Postcode";
                   // }
                    if (IsTestJourney(outcomeViewModel))
                        return "../Outcome/Call_999_CheckAnswer";

                    if (outcomeViewModel.Is999Callback)
                        return "../Outcome/Call_999_Callback";

                    if (outcomeViewModel.OutcomeGroup.Equals(OutcomeGroup.AccidentAndEmergency)) {

                        if (!outcomeViewModel.DosCheckCapacitySummaryResult.IsValidationRequery && outcomeViewModel.DosCheckCapacitySummaryResult.HasITKServices && !outcomeViewModel.HasAcceptedCallbackOffer.HasValue)
                            return "../Outcome/SP_Accident_and_emergency_callback";
                    }
                    if (ViewExists(viewFilePath, context))
                    {
                        _userZoomDataBuilder.SetFieldsForOutcome(model);
                        return viewFilePath;
                    }
                    throw new ArgumentOutOfRangeException(string.Format("Outcome group {0} for outcome {1} has no view configured", model.OutcomeGroup.ToString(), model.Id));
                case NodeType.DeadEndJump:
                    _userZoomDataBuilder.SetFieldsForOutcome(model);
                    return "../Outcome/DeadEndJump";
                case NodeType.PathwaySelectionJump:
                    _userZoomDataBuilder.SetFieldsForOutcome(model);
                    return "../Outcome/PathwaySelectionJump";
                case NodeType.CareAdvice:
                    _userZoomDataBuilder.SetFieldsForCareAdvice(model);
                    return "../Question/InlineCareAdvice";
                case NodeType.Question:
                default:
                    _userZoomDataBuilder.SetFieldsForQuestion(model);
                    return "../Question/Question";
            }
        }

        private bool IsTestJourney(OutcomeViewModel model) {
            if (!string.IsNullOrEmpty(model.TriggerQuestionNo)) //have we already seen the trigger question screen?
                return false;
            var testJourneys = ReadTestJourneys();

            foreach (var testJourney in testJourneys) {
                var result = JsonConvert.DeserializeObject<OutcomeViewModel>(testJourney.Json);
                if (_journeyViewModelComparer.Equals(model, result)) {
                    model.TriggerQuestionNo = testJourney.TriggerQuestionNo;
                    model.TriggerQuestionAnswer = model.Journey.Steps
                        .First(a => a.QuestionNo == model.TriggerQuestionNo).Answer.Title;
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<TestJourneyElement> ReadTestJourneys() {
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
        string GetViewName(JourneyViewModel model, ControllerContext context);
        string GetOutcomeViewPath(OutcomeViewModel model, ControllerContext context, string nextView);
        string GetCallbackFailureViewName(OutcomeGroup outcomeGroup);
        string GetCallbackDuplicateViewName(OutcomeGroup outcomeGroup);
        string GetCallbackConfirmationViewName(OutcomeGroup outcomeGroup);
        string GetServiceUnavailableViewName(OutcomeGroup outcomeGroup);
    }
}
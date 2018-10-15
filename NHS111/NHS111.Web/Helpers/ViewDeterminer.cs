using System;
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
    using Newtonsoft.Json;
    using Presentation.Configuration;

    public class ViewRouter : IViewRouter
    {
        private readonly IAuditLogger _auditLogger;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;

        public ViewRouter(IAuditLogger auditLogger, IUserZoomDataBuilder userZoomDataBuilder)
        {
            _auditLogger = auditLogger;
            _userZoomDataBuilder = userZoomDataBuilder;
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

        public string GetViewName(JourneyViewModel model, ControllerContext context)
        {
            if (model == null) return "../Question/Question";

            switch (model.NodeType)
            {
                case NodeType.Outcome:
                    var viewFilePath = "../Outcome/" + model.OutcomeGroup.Id;
                    //if (model.OutcomeGroup.IsPostcodeFirst())
                    //{
                    //    model.UserInfo.CurrentAddress.IsPostcodeFirst = true;
                    //    _auditLogger.LogEventData(model, "Postcode first journey started");

                    //    viewFilePath = "../PostcodeFirst/Postcode";
                   // }
                    var outcomeViewModel = model as OutcomeViewModel;
                    if (IsTestJourney(outcomeViewModel) && outcomeViewModel.DosCheckCapacitySummaryResult.ResultListEmpty && !outcomeViewModel.DosCheckCapacitySummaryResult.ServicesUnavailable)
                        return "../Outcome/Call_999_CheckAnswer";

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
            if (!string.IsNullOrEmpty(model.TriggerQuestionNo))
                return false; //temp hack
            var testJourneys = ReadTestJourneys();

            //var json = "{\"PathwayNo\":\"PW755\", \"UserInfo\":{ \"Demography\":{ \"Gender\":\"Male\",\"Age\":22}},\"Journey\":{\"Steps\":[{\"questionId\":\"Tx123\",\"answer\":{ \"title\":\"Yes\" }}, {\"questionId\":\"Tx123\",\"answer\":{ \"title\":\"Yes\" }}]}}";
            var comparer = new JourneyViewModelEqualityComparer();
            foreach (var testJourney in testJourneys) {
                var result = JsonConvert.DeserializeObject<OutcomeViewModel>(testJourney.Json);
                if (comparer.Equals(model, result)) {
                    model.TriggerQuestionNo = testJourney.TriggerQuestionNo;
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
    }
}
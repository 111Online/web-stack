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
                    if (IsTestJourney(model))
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

        private bool IsTestJourney(JourneyViewModel model) {
            var testJourneys = ReadTestJourneys();

            //var json = "{\"PathwayNo\":\"PW755\", \"UserInfo\":{ \"Demography\":{ \"Gender\":\"Male\",\"Age\":22}},\"Journey\":{\"Steps\":[{\"questionId\":\"Tx123\",\"answer\":{ \"title\":\"Yes\" }}, {\"questionId\":\"Tx123\",\"answer\":{ \"title\":\"Yes\" }}]}}";
            var comparer = new JourneyViewModelEqualityComparer();
            foreach (var testJourney in testJourneys) {
                var result = JsonConvert.DeserializeObject<JourneyViewModel>(testJourney);
                if (comparer.Equals(model, result))
                    return true;
            }

            return false;
        }

        private static IEnumerable<string> ReadTestJourneys() {
            var section = ConfigurationManager.GetSection("testJourneySection");
            if (!(section is TestJourneysConfigurationSection))
                return new List<string>();

            return (section as TestJourneysConfigurationSection)
                .TestJourneys
                .Cast<TestJourneyElement>()
                .Select(e => e.Json);
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
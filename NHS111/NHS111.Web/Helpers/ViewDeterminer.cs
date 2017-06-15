using System;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Helpers
{
    public interface IViewRouter
    {
        string DetermineViewName(JourneyViewModel model, ControllerContext context);
        string DetermineOutcomeView(JourneyViewModel model, ControllerContext context);
    }

    public class ViewRouter : IViewRouter
    {
        private readonly IAuditLogger _auditLogger;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;

        public ViewRouter(IAuditLogger auditLogger, IUserZoomDataBuilder userZoomDataBuilder)
        {
            _auditLogger = auditLogger;
            _userZoomDataBuilder = userZoomDataBuilder;
        }

        public string DetermineOutcomeView(JourneyViewModel model, ControllerContext context)
        {
            throw new NotImplementedException();
        }

        public string DetermineViewName(JourneyViewModel model, ControllerContext context)
        {
            if (model == null) return "../Question/Question";

            switch (model.NodeType)
            {
                case NodeType.Outcome:
                    if (model.OutcomeGroup.IsPostcodeFirst())
                    {
                        model.UserInfo.CurrentAddress.IsPostcodeFirst = true;
                        _auditLogger.LogEventData(model, "Postcode first journey started");
                    }

                    var viewFilePath = model.OutcomeGroup.IsPostcodeFirst() ? "../PostcodeFirst/Postcode" : "../Outcome/" + model.OutcomeGroup.Id;
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

        private bool ViewExists(string name, ControllerContext context)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(context, name, null);
            return (result.View != null);
        }
    }
}
using System;
using System.EnterpriseServices;
using System.Web.Http;
using NHS111.Features;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
    using Models.Models.Web;
    using Models.Models.Web.DosRequests;
    using Presentation.Builders;

    public class PostcodeFirstController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly IAuditLogger _auditLogger;
        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;
        private readonly IViewRouter _viewRouter;
        private readonly IPostcodePrefillFeature _postcodePrefillFeature;

        public PostcodeFirstController(IDOSBuilder dosBuilder, IAuditLogger auditLogger, IPostCodeAllowedValidator postCodeAllowedValidator, IViewRouter viewRouter, IPostcodePrefillFeature postcodePrefillFeature)
        {
            _dosBuilder = dosBuilder;
            _auditLogger = auditLogger;
            _postCodeAllowedValidator = postCodeAllowedValidator;
            _viewRouter = viewRouter;
            _postcodePrefillFeature = postcodePrefillFeature;
        }

        [HttpPost]
        public async Task<ActionResult> Postcode(OutcomeViewModel model) {
            if (_postcodePrefillFeature.IsEnabled && _postcodePrefillFeature.RequestIncludesPostcode(Request)) {
                model.UserInfo.CurrentAddress.Postcode = _postcodePrefillFeature.GetPostcode(Request);
                return await Outcome(model, null, null, null);
            }

            ModelState.Clear();
            model.UserInfo.CurrentAddress.IsPostcodeFirst = false;
            await _auditLogger.LogEventData(model, "User entered postcode on second request");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Outcome(OutcomeViewModel model, [FromUri] DateTime? overrideDate, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint)
        {
            const string outcomeView = "Outcome";
            const string servicesView = "Services";

            if (!ModelState.IsValidField("UserInfo.CurrentAddress.PostCode")) return View("Postcode", model);

            var dosViewModel = Mapper.Map<DosViewModel>(model);
            
            if (overrideDate.HasValue) dosViewModel.DispositionTime = overrideDate.Value;

            if (string.IsNullOrEmpty(model.UserInfo.CurrentAddress.Postcode))
            {
                await _auditLogger.LogEventData(model, "User did not enter a postcode");
                return View(_viewRouter.GetOutcomeViewPath(model, ControllerContext,outcomeView), model);
            }

            model.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(model.UserInfo.CurrentAddress.Postcode);

            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                await _auditLogger.LogEventData(model, "User entered a postcode outside of pilot area");
                return View(_viewRouter.GetOutcomeViewPath(model, ControllerContext, outcomeView), model);
            }

            await _auditLogger.LogDosRequest(model, dosViewModel);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            model.DosCheckCapacitySummaryResult.ServicesUnavailable = model.DosCheckCapacitySummaryResult.ResultListEmpty;

            if(!model.DosCheckCapacitySummaryResult.ResultListEmpty)
                model.GroupedDosServices =_dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);

            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null && !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                if (model.UserInfo.CurrentAddress.IsPostcodeFirst)
                    return View(_viewRouter.GetOutcomeViewPath(model, ControllerContext, outcomeView), model);
                else
                    return View(_viewRouter.GetOutcomeViewPath(model, ControllerContext, servicesView), model);
            }
            else if (model.DosCheckCapacitySummaryResult.Error != null || model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                return View(_viewRouter.GetOutcomeViewPath(model, ControllerContext, outcomeView), model);
            }
            else
            {
                //present screen with validation errors
                return View(_viewRouter.GetOutcomeViewPath(model, ControllerContext, outcomeView), model);
            }
        }
    }
}
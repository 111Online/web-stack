using System;
using System.Web.Http;
using NHS111.Features;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
    using Models.Models.Web;
    using Presentation.Builders;

    public class PostcodeFirstController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly IAuditLogger _auditLogger;
        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;

        public PostcodeFirstController(IDOSBuilder dosBuilder, IAuditLogger auditLogger, IPostCodeAllowedValidator postCodeAllowedValidator)
        {
            _dosBuilder = dosBuilder;
            _auditLogger = auditLogger;
            _postCodeAllowedValidator = postCodeAllowedValidator;
        }

        [HttpPost]
        public async Task<ActionResult> Postcode(OutcomeViewModel model)
        {
            ModelState.Clear();
            model.UserInfo.CurrentAddress.IsPostcodeFirst = false;
            await _auditLogger.LogEventData(model, "User entered postcode on second request");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Outcome(OutcomeViewModel model, [FromUri] DateTime? overrideDate, [FromUri] bool disableFilter = false)
        {
            if (!ModelState.IsValidField("UserInfo.CurrentAddress.PostCode")) return View("Postcode", model);

            var dosViewModel = Mapper.Map<DosViewModel>(model);
            
            if (overrideDate.HasValue) dosViewModel.DispositionTime = overrideDate.Value;

            if (string.IsNullOrEmpty(model.UserInfo.CurrentAddress.Postcode))
            {
                await _auditLogger.LogEventData(model, "User did not enter a postcode");
                return View("Outcome", model);
            }

            model.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(model.UserInfo.CurrentAddress.Postcode);

            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                await _auditLogger.LogEventData(model, "User entered a postcode outside of pilot area");
                return View("Outcome", model);
            }

            await _auditLogger.LogDosRequest(model, dosViewModel);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel);
            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null && !model.DosCheckCapacitySummaryResult.HasNoServices)
            {
                return model.UserInfo.CurrentAddress.IsPostcodeFirst ? View("Outcome", model) : View("Services", model);
            }
            else if (model.DosCheckCapacitySummaryResult.Error == null && model.DosCheckCapacitySummaryResult.HasNoServices)
            {
                return View("Outcome", model);
            }
            else
            {
                //present screen with validation errors
                return View("Postcode", model);
            }
        }
    }
}
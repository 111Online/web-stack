using System;
using System.Web.Http;
using NHS111.Features;
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
        private readonly IDOSFilteringToggleFeature _dosFilteringToggle;
        private readonly IAuditLogger _auditLogger;

        public PostcodeFirstController(IDOSBuilder dosBuilder, IDOSFilteringToggleFeature dosFilteringToggle, IAuditLogger auditLogger)
        {
            _dosBuilder = dosBuilder;
            _dosFilteringToggle = dosFilteringToggle;
            _auditLogger = auditLogger;
        }

        [HttpPost]
        public async Task<ActionResult> Outcome(OutcomeViewModel model, [FromUri] DateTime? overrideDate, [FromUri] bool disableFilter = false)
        {
            if (!ModelState.IsValidField("UserInfo.CurrentAddress.PostCode")) return View("Postcode", model);

            var dosViewModel = Mapper.Map<DosViewModel>(model);
            if (_dosFilteringToggle.IsEnabled)
            {
                if (overrideDate.HasValue) dosViewModel.DispositionTime = overrideDate.Value;
            }

            if(string.IsNullOrEmpty(model.UserInfo.CurrentAddress.Postcode))
                return View("Outcome", model);

            await _auditLogger.LogDosRequest(model, dosViewModel);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel);
            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null && !model.DosCheckCapacitySummaryResult.HasNoServices)
                return View("Outcome", model);

            return View("Postcode", model);
        }
    }
}


namespace NHS111.Web.Controllers {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
    using Models.Models.Domain;
    using Models.Models.Web;
    using Newtonsoft.Json;
    using Presentation.Builders;
    using Presentation.Logging;
    using Utils.Attributes;
    using Utils.Filters;

    [LogHandleErrorForMVC]
    public class OutcomeController : Controller {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IDOSBuilder _dosBuilder;
        private readonly ISurgeryBuilder _surgeryBuilder;
        private readonly ILocationResultBuilder _locationResultBuilder;
        private readonly IAuditLogger _auditLogger;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder,
            ISurgeryBuilder surgeryBuilder, ILocationResultBuilder locationResultBuilder, IAuditLogger auditLogger) {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
            _surgeryBuilder = surgeryBuilder;
            _locationResultBuilder = locationResultBuilder;
            _auditLogger = auditLogger;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input) {
            return Json((await _surgeryBuilder.SearchSurgeryBuilder(input)));
        }

        [HttpPost]
        public async Task<JsonResult> PostcodeLookup(string postCode) {
            var locationResults = await _locationResultBuilder.LocationResultByPostCodeBuilder(postCode);
            return Json(Mapper.Map<List<AddressInfoViewModel>>(locationResults));
        }

        [HttpGet]
        [Route("outcome/disposition/{age?}/{gender?}/{dxCode?}/{symptomGroup?}/{symptomDiscriminator?}")]
        public ActionResult Disposition(int? age, string gender, string dxCode, string symptomGroup,
            string symptomDiscriminator) {
            var DxCode = new DispositionCode(dxCode ?? "Dx38");
            var Gender = new Gender(gender ?? "Male");

            var model = new OutcomeViewModel {
                Id = DxCode.Value,
                UserInfo = new UserInfo {
                    Age = age ?? 38,
                    Gender = Gender.Value
                },
                SymptomGroup = symptomGroup ?? "1203",
                SymptomDiscriminatorCode = symptomDiscriminator ?? "4003",
                AddressInfoViewModel = new AddressInfoViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceList(OutcomeViewModel model) {
            var dosViewModel = Mapper.Map<DosViewModel>(model);
            AuditDosRequest(model, dosViewModel);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel);
            AuditDosResponse(model);
            return View("ServiceList", model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails(OutcomeViewModel model) {
            var dosCase = Mapper.Map<DosViewModel>(model);
            AuditDosRequest(model, dosCase);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase);
            AuditDosResponse(model);
            return View("ServiceDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(OutcomeViewModel model) {
            AuditSelectedService(model);

            model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
            return View("PersonalDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> Confirmation(OutcomeViewModel model) {
            model = await _outcomeViewModelBuilder.ItkResponseBuilder(model);
            if (model.ItkSendSuccess.HasValue && model.ItkSendSuccess.Value)
                return View(model);
            return View("ServiceBookingFailure", model);
        }

        [HttpPost]
        public ActionResult Emergency() {
            return View();
        }

        private void AuditDosRequest(OutcomeViewModel model, DosViewModel dosViewModel) {
            var audit = model.ToAuditEntry();
            audit.DosRequest = JsonConvert.SerializeObject(dosViewModel);
            _auditLogger.Log(audit);
        }

        private void AuditDosResponse(OutcomeViewModel model) {
            var audit = model.ToAuditEntry();
            audit.DosResponse = JsonConvert.SerializeObject(model.DosCheckCapacitySummaryResult);
            _auditLogger.Log(audit);
        }

        private void AuditSelectedService(OutcomeViewModel model) {
            var audit = model.ToAuditEntry();
            audit.EventData = string.Format("User selected service '{0}' ({1})", model.SelectedService.Name, model.SelectedService.Id);
            _auditLogger.Log(audit);
        }
    }
}
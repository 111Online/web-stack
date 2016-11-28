

using System;
using System.Web.Http;
using NHS111.Features;

namespace NHS111.Web.Controllers {
    using System.Collections.Generic;
    using System.IO;
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
        private readonly Presentation.Configuration.IConfiguration _configuration;
        private readonly IDOSFilteringToggleFeature _dosFilteringToggle;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder,
            ISurgeryBuilder surgeryBuilder, ILocationResultBuilder locationResultBuilder, IAuditLogger auditLogger, Presentation.Configuration.IConfiguration configuration, IDOSFilteringToggleFeature dosFilteringToggle)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
            _surgeryBuilder = surgeryBuilder;
            _locationResultBuilder = locationResultBuilder;
            _auditLogger = auditLogger;
            _configuration = configuration;
            _dosFilteringToggle = dosFilteringToggle;
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
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    { 
                        Age = age ?? 38,
                        Gender = Gender.Value
                    }
                },
                SymptomGroup = symptomGroup ?? "1203",
                SymptomDiscriminatorCode = symptomDiscriminator ?? "4003",
                AddressInfoViewModel = new PersonalInfoAddressViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceList(OutcomeViewModel model,  [FromUri]DateTime? overrideDate, [FromUri]bool disableFilter = false)
        {
            var dosViewModel = Mapper.Map<DosViewModel>(model);
            if (_dosFilteringToggle.IsEnabled)
            {
                if (overrideDate.HasValue) dosViewModel.DispositionTime = overrideDate.Value;
            }
            
            AuditDosRequest(model, dosViewModel);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel);
            AuditDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null)
                return View("ServiceList", model);

            return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails(OutcomeViewModel model) {
            var dosCase = Mapper.Map<DosViewModel>(model);
            AuditDosRequest(model, dosCase);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase);
            AuditDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null)
                return View("ServiceDetails", model);

            return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(OutcomeViewModel model) {
            ModelState.Clear();
            AuditSelectedService(model);

            model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
            return View("PersonalDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> Confirmation(OutcomeViewModel model) {
            if (!ModelState.IsValid) return View("PersonalDetails", model);
            model = await _outcomeViewModelBuilder.ItkResponseBuilder(model);
            if (model.ItkSendSuccess.HasValue && model.ItkSendSuccess.Value)
                return View(model);
            return View("ServiceBookingFailure", model);
        }

        [HttpPost]
        public ActionResult GetDirections(OutcomeViewModel model, int selectedServiceId, string selectedServiceName, string selectedServiceAddress)
        {
            AuditSelectedService(model, selectedServiceName, selectedServiceId);

            return Redirect(string.Format(_configuration.MapsApiUrl, selectedServiceName, selectedServiceAddress));
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

        private async Task AuditSelectedService(OutcomeViewModel model, string selectedServiceName, int selectedServiceId)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = FormatEventData(selectedServiceName, selectedServiceId);
            _auditLogger.Log(audit);
        }

        private void AuditSelectedService(OutcomeViewModel model)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = FormatEventData(model.SelectedService.Name, model.SelectedService.Id);
            _auditLogger.Log(audit);
        }

        private string FormatEventData(string selectedServiceName, int selectedServiceId)
        {
            return string.Format("User selected service '{0}' ({1})", selectedServiceName, selectedServiceId);
        }
    }
}
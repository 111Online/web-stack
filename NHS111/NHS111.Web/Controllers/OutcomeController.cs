

using System;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using NHS111.Features;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Web.Controllers
{
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
    using System.Web;

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
        public async Task<JsonResult> PostcodeLookup(string postCode)
        {
            var locationResults = await GetPostcodeResults(postCode);
            return Json((locationResults));
        }

        private async Task<List<AddressInfoViewModel>> GetPostcodeResults(string postCode)
        {
            //TODO: Add timeout, so we don't wait too long!
            var results = await _locationResultBuilder.LocationResultByPostCodeBuilder(postCode);
            return Mapper.Map<List<AddressInfoViewModel>>(results);
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
                AddressInfoViewModel = new PersonalDetailsAddressViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceList(OutcomeViewModel model,  [FromUri]DateTime? overrideDate, [FromUri]bool disableFilter = false)
        {
            if (!ModelState.IsValidField("UserInfo.CurrentAddress.PostCode")) return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);

            model.DosCheckCapacitySummaryResult = await GetServiceAvailability(model, overrideDate);
            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null && !model.DosCheckCapacitySummaryResult.HasNoServices)
                return View("ServiceList", model);

            return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);
        }

        private async Task<DosCheckCapacitySummaryResult> GetServiceAvailability(OutcomeViewModel model, DateTime? overrideDate)
        {
            var dosViewModel = Mapper.Map<DosViewModel>(model);
            if (_dosFilteringToggle.IsEnabled)
            {
                if (overrideDate.HasValue) dosViewModel.DispositionTime = overrideDate.Value;
            }

           await _auditLogger.LogDosRequest(model, dosViewModel);
           return await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails(OutcomeViewModel model) {

            if (!ModelState.IsValidField("UserInfo.CurrentAddress.Postcode")) return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);

            var dosCase = Mapper.Map<DosViewModel>(model);
            await _auditLogger.LogDosRequest(model, dosCase);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase);
            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null)
                return View("ServiceDetails", model);

            return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(OutcomeViewModel model) {
            ModelState.Clear();
            await _auditLogger.LogSelectedService(model);

            model = await PopulateAddressPickerFields(model);

            return View("PersonalDetails", model);
        }

        private async Task<OutcomeViewModel> PopulateAddressPickerFields(OutcomeViewModel model)
        {
            //map postcode to field to submit to ITK (preventing multiple entries of same data)
            model.AddressInfoViewModel.PreviouslyEnteredPostcode = model.UserInfo.CurrentAddress.Postcode;

            //pre-populate picker fields from postcode lookup service
            var postcodes = await GetPostcodeResults(model.AddressInfoViewModel.PreviouslyEnteredPostcode);
            var firstSelectItemText = postcodes.Count + " addresses found. Please choose...";
            var items = new List<SelectListItem> { new SelectListItem { Text = firstSelectItemText, Value = "0", Selected = true } };
            items.AddRange(postcodes.Select(postcode => new SelectListItem { Text = postcode.AddressLine1, Value = postcode.UPRN }).ToList());
            model.AddressInfoViewModel.AddressPicker = items;

            model.AddressInfoViewModel.AddressOptions = new JavaScriptSerializer().Serialize(Json(postcodes).Data);

            return model;
        }

        [HttpPost]
        public async Task<ActionResult> Confirmation(OutcomeViewModel model) {
            if (!ModelState.IsValid)
            {
                //populate address picker fields
                model = await PopulateAddressPickerFields(model);
                return View("PersonalDetails", model);
            }
            var availiableServices = await GetServiceAvailability(model, DateTime.Now);
            _auditLogger.LogDosResponse(model);
            if (SelectedServiceExits(model.SelectedService.Id, availiableServices))
            {
                model = await _outcomeViewModelBuilder.ItkResponseBuilder(model);
                if (model.ItkSendSuccess.HasValue && model.ItkSendSuccess.Value)
                    return View(model);
                return model.ItkDuplicate.HasValue && model.ItkDuplicate.Value ? View("DuplicateBookingFailure", model) : View("ServiceBookingFailure", model);
            }
            model.UnavailableSelectedService = model.SelectedService;
            model.DosCheckCapacitySummaryResult = availiableServices;
            return View("ServieBookingUnavailable", model);
        }

        private bool SelectedServiceExits(int selectedServiceId, DosCheckCapacitySummaryResult availiableServices)
        {
            return !availiableServices.HasNoServices && availiableServices.Success.Services.Exists(s => s.Id == selectedServiceId);
        }

        [HttpPost]
        public ActionResult GetDirections(OutcomeViewModel model, int selectedServiceId, string selectedServiceName, string selectedServiceAddress)
        {
            _auditLogger.LogEventData(model, string.Format("User selected service '{0}' ({1})", selectedServiceName, selectedServiceId));
            return Redirect(string.Format(_configuration.MapsApiUrl, selectedServiceName, selectedServiceAddress));
        }

        [HttpPost]
        public ActionResult Emergency() {
            return View();
        }
    }
}
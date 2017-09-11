using System;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using NHS111.Features;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Logging;
using NHS111.Models.Models.Web.Validators;
using DayOfWeek = System.DayOfWeek;

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
    using Models.Models.Web.DosRequests;

    [LogHandleErrorForMVC]
    public class OutcomeController : Controller {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IDOSBuilder _dosBuilder;
        private readonly ISurgeryBuilder _surgeryBuilder;
        private readonly ILocationResultBuilder _locationResultBuilder;
        private readonly IAuditLogger _auditLogger;
        private readonly Presentation.Configuration.IConfiguration _configuration;
        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder,
            ISurgeryBuilder surgeryBuilder, ILocationResultBuilder locationResultBuilder, IAuditLogger auditLogger, Presentation.Configuration.IConfiguration configuration, IPostCodeAllowedValidator postCodeAllowedValidator)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
            _surgeryBuilder = surgeryBuilder;
            _locationResultBuilder = locationResultBuilder;
            _auditLogger = auditLogger;
            _configuration = configuration;
            _postCodeAllowedValidator = postCodeAllowedValidator;
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

            var model = new OutcomeViewModel() {
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

        public void AutoSelectFirstItkService(OutcomeViewModel model)
        {
            var service = model.DosCheckCapacitySummaryResult.Success.Services.FirstOrDefault(s => s.CallbackEnabled);
            
            if (service != null)
                model.SelectedServiceId = service.Id.ToString();
        }

        [HttpPost]
        public async Task<ActionResult> ServiceList([Bind(Prefix = "FindService")]OutcomeViewModel model,  [FromUri] DateTime? overrideDate, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint)
        {
            if (!ModelState.IsValidField("FindService.UserInfo.CurrentAddress.PostCode"))
                return View(model.CurrentView, model);
            
            if (!_postCodeAllowedValidator.IsAllowedPostcode(model.UserInfo.CurrentAddress.Postcode))
            {
                ModelState.AddModelError("FindService.UserInfo.CurrentAddress.Postcode", "Sorry, this service is not currently available in your area.  Please call NHS 111 for advice now");
                return View(model.CurrentView, model);
            }

            model.DosCheckCapacitySummaryResult = await GetServiceAvailability(model, overrideDate, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null &&
                !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                if (model.OutcomeGroup.IsAutomaticSelectionOfItkResult())
                {
                    AutoSelectFirstItkService(model);
                    if (model.SelectedService != null)
                        return await PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model));
                }
                
                return View("~\\Views\\Outcome\\ServiceList.cshtml", model);
            }

            return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);
        }

        private async Task<DosCheckCapacitySummaryResult> GetServiceAvailability(OutcomeViewModel model, DateTime? overrideDate, bool filterServices, DosEndpoint? endpoint)
        {
            var dosViewModel = Mapper.Map<DosViewModel>(model);
                if (overrideDate.HasValue) dosViewModel.DispositionTime = overrideDate.Value;

           await _auditLogger.LogDosRequest(model, dosViewModel);
           return await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, filterServices, endpoint);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails([Bind(Prefix = "FindService")]OutcomeViewModel model, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint) {

            if (!ModelState.IsValidField("FindService.UserInfo.CurrentAddress.Postcode"))
                return View(model.CurrentView, model);

            if (!_postCodeAllowedValidator.IsAllowedPostcode(model.UserInfo.CurrentAddress.Postcode))
            {
                ModelState.AddModelError("FindService.UserInfo.CurrentAddress.Postcode", "Sorry, this service is not currently available in your area.  Please call NHS 111 for advice now");
                return View(model.CurrentView, model);
            }

            var dosCase = Mapper.Map<DosViewModel>(model);
            await _auditLogger.LogDosRequest(model, dosCase);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            await _auditLogger.LogDosResponse(model);

            if (model.DosCheckCapacitySummaryResult.Error == null &&
                !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                if (model.OutcomeGroup.IsAutomaticSelectionOfItkResult())
                {
                    AutoSelectFirstItkService(model);
                    if (model.SelectedService != null)
                        return await PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model));
                }
                    return View("~\\Views\\Outcome\\ServiceDetails.cshtml", model);
                    //explicit path to view because, when direct-linking, the route is no longer /outcome causing convention based view lookup to fail    
            }

            return View(Path.GetFileNameWithoutExtension(model.CurrentView), model);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(PersonalDetailViewModel model)
        {
            ModelState.Clear();
            await _auditLogger.LogSelectedService(model);

            model = await PopulateAddressPickerFields(model);

            return View("~\\Views\\Outcome\\PersonalDetails.cshtml", model);
        }

        private async Task<PersonalDetailViewModel> PopulateAddressPickerFields(PersonalDetailViewModel model)
        {
            //map postcode to field to submit to ITK (preventing multiple entries of same data)
            model.AddressInfoViewModel.PreviouslyEnteredPostcode = model.UserInfo.CurrentAddress.Postcode;

            //pre-populate picker fields from postcode lookup service
            var postcodes = await GetPostcodeResults(model.AddressInfoViewModel.PreviouslyEnteredPostcode);
            var firstSelectItemText = postcodes.Count + " addresses found. Please choose...";
            var items = new List<SelectListItem> { new SelectListItem { Text = firstSelectItemText, Value = "", Selected = true } };
            items.AddRange(postcodes.Select(postcode => new SelectListItem { Text = postcode.AddressLine1, Value = postcode.UPRN }).ToList());
            model.AddressInfoViewModel.AddressPicker = items;

            model.AddressInfoViewModel.AddressOptions = new JavaScriptSerializer().Serialize(Json(postcodes).Data);

            return model;
        }

        [HttpPost]
        public async Task<ActionResult> Confirmation(PersonalDetailViewModel model, [FromUri] bool? overrideFilterServices)
        {
            if (!ModelState.IsValid)
            {
                //populate address picker fields
                model = await PopulateAddressPickerFields(model);
                return View("PersonalDetails", model);
            }
            var availableServices = await GetServiceAvailability(model, DateTime.Now, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, null);
            _auditLogger.LogDosResponse(model);
            if (SelectedServiceExits(model.SelectedService.Id, availableServices))
            {
               var outcomeViewModel  = ConvertPatientInformantDateToUserinfo(model.PatientInformantDetails, model);
               outcomeViewModel = await _outcomeViewModelBuilder.ItkResponseBuilder(outcomeViewModel);
               if (outcomeViewModel.ItkSendSuccess.HasValue && outcomeViewModel.ItkSendSuccess.Value)
                   return View(outcomeViewModel);
               return outcomeViewModel.ItkDuplicate.HasValue && outcomeViewModel.ItkDuplicate.Value ? View("DuplicateBookingFailure", outcomeViewModel) : View("ServiceBookingFailure", outcomeViewModel);
            }

            model.UnavailableSelectedService = model.SelectedService;
            model.DosCheckCapacitySummaryResult = availableServices;
            model.DosCheckCapacitySummaryResult.ServicesUnavailable = availableServices.ResultListEmpty;
            model.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(model.UserInfo.CurrentAddress.Postcode);
            
            return View("ServiceBookingUnavailable", model);
        }

        private OutcomeViewModel ConvertPatientInformantDateToUserinfo(PatientInformantViewModel patientInformantModel, OutcomeViewModel model)
        {
            if (patientInformantModel.Informant == InformantType.Self)
            {
                model.UserInfo.FirstName = patientInformantModel.SelfName.Forename;
                model.UserInfo.LastName = patientInformantModel.SelfName.Surname;
                model.Informant.IsInformantForPatient = false;
            }

            if (patientInformantModel.Informant == InformantType.ThirdParty)
            {
                model.UserInfo.FirstName = patientInformantModel.PatientName.Forename;
                model.UserInfo.LastName = patientInformantModel.PatientName.Surname;

                model.Informant.Forename = patientInformantModel.InformantName.Forename;
                model.Informant.Surname = patientInformantModel.InformantName.Surname;
                model.Informant.IsInformantForPatient = true;
            }
            return model;
        }

        private bool SelectedServiceExits(int selectedServiceId, DosCheckCapacitySummaryResult availableServices)
        {
            return !availableServices.ResultListEmpty && availableServices.Success.Services.Exists(s => s.Id == selectedServiceId);
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
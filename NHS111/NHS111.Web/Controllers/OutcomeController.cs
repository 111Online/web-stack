using System;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using NHS111.Features;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Logging;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Helpers;
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
    using System.Text.RegularExpressions;
    using Models.Models.Web.Enums;

    [LogHandleErrorForMVC]
    public class OutcomeController : Controller
    {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IDOSBuilder _dosBuilder;
        private readonly ISurgeryBuilder _surgeryBuilder;
        private readonly ILocationResultBuilder _locationResultBuilder;
        private readonly IAuditLogger _auditLogger;
        private readonly Presentation.Configuration.IConfiguration _configuration;
        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;
        private readonly IViewRouter _viewRouter;
        private readonly IReferralResultBuilder _referralResultBuilder;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder,
            ISurgeryBuilder surgeryBuilder, ILocationResultBuilder locationResultBuilder, IAuditLogger auditLogger, Presentation.Configuration.IConfiguration configuration, IPostCodeAllowedValidator postCodeAllowedValidator, IViewRouter viewRouter, IReferralResultBuilder referralResultBuilder)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
            _surgeryBuilder = surgeryBuilder;
            _locationResultBuilder = locationResultBuilder;
            _auditLogger = auditLogger;
            _configuration = configuration;
            _postCodeAllowedValidator = postCodeAllowedValidator;
            _viewRouter = viewRouter;
            _referralResultBuilder = referralResultBuilder;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _surgeryBuilder.SearchSurgeryBuilder(input)));
        }

        [HttpPost]
        public async Task<ActionResult> ChangePostcode(OutcomeViewModel model)
        {
            ModelState.Clear();
            await _auditLogger.LogEventData(model, "User elected to change postcode.");
            return View(model); ;
        }

        [HttpPost]
        public async Task<ActionResult> DispositionWithServices(OutcomeViewModel model, string submitAction, DosEndpoint? endpoint = null, DateTime? dosSearchTime = null)
        {
            if (submitAction == "manualpostcode") return View("ChangePostcode", model);
            var postcodeValidatorResponse = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode);

            model.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidatorResponse == PostcodeValidatorResponse.InPathwaysArea;
            if(postcodeValidatorResponse == PostcodeValidatorResponse.InvalidSyntax)
            {
                ModelState.AddModelError("CurrentPostcode", "Enter a valid postcode.");
                return View("../Outcome/ChangePostcode", model);
            }
            if (postcodeValidatorResponse == PostcodeValidatorResponse.PostcodeNotFound)
            {
                ModelState.AddModelError("CurrentPostcode", "We can't find any services in '" + model.CurrentPostcode +"'. Check the postcode is correct.");
                return View("../Outcome/ChangePostcode", model);
            }
            model.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode) == PostcodeValidatorResponse.InPathwaysArea;
            
            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                return View("../Outcome/OutOfArea", model);
            }

            var outcomeModel = await _outcomeViewModelBuilder.PopulateGroupedDosResults(model, dosSearchTime, null, endpoint);
            var viewName = _viewRouter.GetViewName(model, ControllerContext);

            return View(viewName, outcomeModel);
        }

        [HttpPost]
        public ActionResult OutcomeWithoutResults(OutcomeViewModel outcomeModel)
        {
            var viewName = _viewRouter.GetViewName(outcomeModel, ControllerContext);
            return View(viewName, outcomeModel);
        }

        [HttpPost]
        public async Task<JsonResult> PostcodeLookup(string postCode)
        {
            var locationResults = await GetPostcodeResults(postCode);
            return Json((locationResults));
        }

        private async Task<AddressInfoCollectionViewModel> GetPostcodeResults(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode)) return AddressInfoCollectionViewModel.InvalidSyntaxResponse;
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(postCode.Replace(" ", ""))) return AddressInfoCollectionViewModel.InvalidSyntaxResponse;

            var results = await _locationResultBuilder.LocationResultValidatedByPostCodeBuilder(postCode);
            return Mapper.Map<AddressInfoCollectionViewModel>(results);
        }

        [HttpGet]
        [Route("outcome/disposition/{age?}/{gender?}/{dxCode?}/{symptomGroup?}/{symptomDiscriminator?}")]
        public ActionResult Disposition(int? age, string gender, string dxCode, string symptomGroup,
            string symptomDiscriminator)
        {
            var DxCode = new DispositionCode(dxCode ?? "Dx38");
            var Gender = new Gender(gender ?? "Male");

            var model = new OutcomeViewModel()
            {
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
                SymptomDiscriminatorCode = symptomDiscriminator ?? "4003"
            };

            return View(model);
        }

        public void AutoSelectFirstItkService(OutcomeViewModel model)
        {
            var service = model.DosCheckCapacitySummaryResult.Success.Services.FirstOrDefault(s => s.OnlineDOSServiceType.IsReferral);

            if (service != null)
                model.SelectedServiceId = service.Id.ToString();
        }

        [HttpPost]
        public async Task<ActionResult> ServiceList([Bind(Prefix = "FindService")]OutcomeViewModel model, [FromUri] DateTime? overrideDate, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint)
        {
            if (!ModelState.IsValidField("FindService.CurrentPostcode"))
                return View(model.CurrentView, model);

            model.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode) == PostcodeValidatorResponse.InPathwaysArea;

            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                ModelState.AddModelError("FindService.CurrentPostcode", "Sorry, this service is not currently available in your area.  Please call NHS 111 for advice now");
                return View(model.CurrentView, model);
            }

            model.DosCheckCapacitySummaryResult = await GetServiceAvailability(model, overrideDate, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            await _auditLogger.LogDosResponse(model, model.DosCheckCapacitySummaryResult);

            model.NodeType = NodeType.Outcome;

            if (model.DosCheckCapacitySummaryResult.Error == null &&
                !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                if (model.OutcomeGroup.Is999NonUrgent && !model.DosCheckCapacitySummaryResult.HasITKServices) {
                    model.CurrentView = _viewRouter.GetViewName(model, this.ControllerContext);
                    return View(model.CurrentView, model);
                }

                model.GroupedDosServices =
                    _dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);

                if (model.OutcomeGroup.IsAutomaticSelectionOfItkResult())
                {
                    AutoSelectFirstItkService(model);
                    if (model.SelectedService != null)
                        return await PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model));
                }

                return View("~\\Views\\Outcome\\ServiceList.cshtml", model);
            }

            if (model.OutcomeGroup.Is999NonUrgent) {
                model.CurrentView = _viewRouter.GetViewName(model, this.ControllerContext);
            }

            return View(model.CurrentView, model);
        }

        private async Task<DosCheckCapacitySummaryResult> GetServiceAvailability(OutcomeViewModel model, DateTime? overrideDate, bool filterServices, DosEndpoint? endpoint)
        {
            var dosViewModel = _dosBuilder.BuildDosViewModel(model, overrideDate);
            await _auditLogger.LogDosRequest(model, dosViewModel);
            return await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, filterServices, endpoint);
        }


        [HttpGet]
        [Route("map/")]
        public ActionResult ServiceMap()
        {
            var model = new OutcomeMapViewModel()
            {
                MapsApiKey = _configuration.MapsApiKey
            };
            return View("~\\Views\\Shared\\_GoogleMap.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails([Bind(Prefix = "FindService")]OutcomeViewModel model, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint)
        {

            if (!ModelState.IsValidField("FindService.CurrentPostcode"))
                return View(model.CurrentView, model);

            var postcodeValidator = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode);

            model.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidator == PostcodeValidatorResponse.InPathwaysArea;

            if (postcodeValidator == PostcodeValidatorResponse.InvalidSyntax)
            {
                ModelState.AddModelError("FindService.CurrentPostcode", "Enter a valid postcode.");
                return View(model.CurrentView, model);

            }
            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                ModelState.AddModelError("FindService.CurrentPostcode", "Sorry, this service is not currently available in your area.  Please call NHS 111 for advice now");
                return View(model.CurrentView, model);
            }

            var dosCase = Mapper.Map<DosViewModel>(model);
            await _auditLogger.LogDosRequest(model, dosCase);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            await _auditLogger.LogDosResponse(model, model.DosCheckCapacitySummaryResult);

            if (model.DosCheckCapacitySummaryResult.Error == null &&
                !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                model.GroupedDosServices =
                    _dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);

                if (model.OutcomeGroup.IsAutomaticSelectionOfItkResult())
                {
                    AutoSelectFirstItkService(model);
                    if (model.SelectedService != null)
                        return await PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model));
                }
                return View("~\\Views\\Outcome\\ServiceDetails.cshtml", model);
                //explicit path to view because, when direct-linking, the route is no longer /outcome causing convention based view lookup to fail
            }

            return View(model.CurrentView, model);
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
            model.AddressInformation.PatientCurrentAddress.PreviouslyEnteredPostcode = model.CurrentPostcode;

            //pre-populate picker fields from postcode lookup service
            var postcodes = await GetPostcodeResults(model.AddressInformation.PatientCurrentAddress.PreviouslyEnteredPostcode);
            if (postcodes.ValidatedPostcodeResponse == PostcodeValidatorResponse.PostcodeNotFound) return model;

            var firstSelectItemText = postcodes.Addresses.Count() + " addresses found. Please choose...";
            var items = new List<SelectListItem>
            {
                new SelectListItem {Text = firstSelectItemText, Value = "", Selected = true}
            };
            items.AddRange(postcodes.Addresses.Select(postcode =>
                new SelectListItem {Text = postcode.FormattedAddress, Value = postcode.UPRN}).ToList());
            model.AddressInformation.PatientCurrentAddress.AddressPicker = items;

            model.AddressInformation.PatientCurrentAddress.AddressOptions =
                new JavaScriptSerializer().Serialize(Json(postcodes).Data);

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
            var availableServices = await GetServiceAvailability(model, null, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, null);
            await _auditLogger.LogDosResponse(model, availableServices);
            if (availableServices.ContainsService(model.SelectedService))
            {
                var outcomeViewModel = ConvertPatientInformantDateToUserinfo(model.PatientInformantDetails, model);
                outcomeViewModel = await _outcomeViewModelBuilder.ItkResponseBuilder(outcomeViewModel);
                var result = _referralResultBuilder.Build(outcomeViewModel);
                return View(result.ViewName, result);
            }

            var unavailableResult = _referralResultBuilder.BuildServiceUnavailableResult(model, availableServices);
            return View(unavailableResult.ViewName, unavailableResult);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmAddress(string longlat, ConfirmLocationViewModel model)
        {
            var results = await _locationResultBuilder.LocationResultByGeouilder(longlat);
            var locationResults = Mapper.Map<List<AddressInfoViewModel>>(results.DistinctBy(r => r.Thoroughfare));
            model.FoundLocations = locationResults;
            return View("ConfirmLocation", model);
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

        [HttpPost]
        public ActionResult GetDirections(OutcomeViewModel model, int selectedServiceId, string selectedServiceName, string selectedServiceAddress)
        {
            _auditLogger.LogEventData(model, string.Format("User selected service '{0}' ({1})", selectedServiceName, selectedServiceId));
            return Redirect(string.Format(_configuration.MapsApiUrl, model.CurrentPostcode, selectedServiceAddress));
        }

        [HttpPost]
        public void LogSelectedService(OutcomeViewModel model, int selectedServiceId, string selectedServiceName, string selectedServiceAddress)
        {
            _auditLogger.LogEventData(model, string.Format("User selected service '{0}' ({1})", selectedServiceName, selectedServiceId));
        }

        [HttpPost]
        public ActionResult Emergency()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EdCallbackAcceptance(PersonalDetailViewModel model, string selectedAnswer) {
            model.HasAcceptedCallbackOffer = selectedAnswer.ToLower() == "yes";

            if (model.HasAcceptedCallbackOffer.Value) {
                AutoSelectFirstItkService(model);
                if (model.SelectedService != null)
                    return await PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model));
            }

            var outcome = await _outcomeViewModelBuilder.DispositionBuilder(model);
            var viewName = _viewRouter.GetViewName(outcome, ControllerContext);
            model.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode) == PostcodeValidatorResponse.InPathwaysArea;

            return View(viewName, outcome);

        }
    }
}

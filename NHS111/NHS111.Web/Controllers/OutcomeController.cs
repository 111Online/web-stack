using Microsoft.Ajax.Utilities;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace NHS111.Web.Controllers
{
    using AutoMapper;
    using Models.Models.Domain;
    using Models.Models.Web;
    using Models.Models.Web.DosRequests;
    using Models.Models.Web.Enums;
    using Presentation.Builders;
    using Presentation.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Utils.Attributes;

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
        private readonly IRecommendedServiceBuilder _recommendedServiceBuilder;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder, ISurgeryBuilder surgeryBuilder,
            ILocationResultBuilder locationResultBuilder, IAuditLogger auditLogger, Presentation.Configuration.IConfiguration configuration,
            IPostCodeAllowedValidator postCodeAllowedValidator, IViewRouter viewRouter, IReferralResultBuilder referralResultBuilder,
            IRecommendedServiceBuilder recommendedServiceBuilder)
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
            _recommendedServiceBuilder = recommendedServiceBuilder;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _surgeryBuilder.SearchSurgeryBuilder(input).ConfigureAwait(false)));
        }

        [HttpPost]
        public async Task<ActionResult> ChangePostcode(OutcomeViewModel model)
        {
            ModelState.Clear();
            _auditLogger.LogEventData(model, "User elected to change postcode.");
            return View(model);
        }

        private OutcomeViewModel PopulateCCGAndStp(OutcomeViewModel model)
        {
            if (_postCodeAllowedValidator.CcgModel == null)
            {
                model.Campaign = string.Empty;
                model.Source = string.Empty;

                return model;
            }

            model.Campaign = _postCodeAllowedValidator.CcgModel.StpName;
            model.Source = _postCodeAllowedValidator.CcgModel.CCG;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult> DispositionWithServices(OutcomeViewModel model, string submitAction, DosEndpoint? endpoint = null, DateTime? dosSearchTime = null)
        {
            ModelState.Clear();
            if (submitAction == "manualpostcode") return View("../Outcome/ChangePostcode", model);
            var postcodeValidatorResponse = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode);

            if (postcodeValidatorResponse == PostcodeValidatorResponse.InvalidSyntax)
            {
                ModelState.AddModelError("CurrentPostcode", "Enter a valid postcode.");
                return View("../Outcome/ChangePostcode", model);
            }
            if (postcodeValidatorResponse == PostcodeValidatorResponse.PostcodeNotFound)
            {
                ModelState.AddModelError("CurrentPostcode", "We can't find any services in '" + model.CurrentPostcode + "'. Check the postcode is correct.");
                return View("../Outcome/ChangePostcode", model);
            }

            model.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidatorResponse.IsInPilotAreaForOutcome(model.OutcomeGroup);
            model = PopulateCCGAndStp(model);

            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                if (model.OutcomeGroup.IsPharmacyGroup)
                    return View("../Pathway/EmergencyPrescriptionsOutOfArea", model);

                return View("../Outcome/OutOfArea", model);
            }

            var outcomeModel = await _outcomeViewModelBuilder.PopulateGroupedDosResults(model, dosSearchTime, null, endpoint).ConfigureAwait(false);
            var viewRouter = _viewRouter.Build(outcomeModel, ControllerContext);

            return View(viewRouter.ViewName, outcomeModel);
        }

        [HttpPost]
        public ActionResult OutcomeWithoutResults(OutcomeViewModel outcomeModel)
        {
            var viewRouter = _viewRouter.Build(outcomeModel, ControllerContext);
            return View(viewRouter.ViewName, outcomeModel);
        }

        [HttpPost]
        public ActionResult RecommendedService(OutcomeViewModel outcomeModel)
        {
            ModelState.Clear();
            outcomeModel.HasSeenPreamble = true;
            var viewRouter = _viewRouter.Build(outcomeModel, ControllerContext);
            return View(viewRouter.ViewName, outcomeModel);
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
        public async Task<ActionResult> ServiceListUnprefixed(OutcomeViewModel model, [FromUri] DateTime? overrideDate, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint)
        {
            return await ServiceList(model, overrideDate, overrideFilterServices, endpoint).ConfigureAwait(false);
        }


        [HttpPost]
        public async Task<ActionResult> ServiceList([Bind(Prefix = "FindService")]OutcomeViewModel model, [FromUri] DateTime? overrideDate, [FromUri] bool? overrideFilterServices, DosEndpoint? endpoint)
        {
            // Set model.OutcomePage to "Other ways to get help" page so that survey link can be created correctly 
            model.OutcomePage = OutcomePage.OtherServices;

            var reason = Request.Form["reason"];
            _auditLogger.LogPrimaryCareReason(model, reason);
            if (Request.Form["OtherServices"] != null)
            {
                _auditLogger.LogPrimaryCareReason(model, "Patient clicked other things you can do");
            }

            if (!ModelState.IsValidField("FindService.CurrentPostcode"))
                return View(model.CurrentView, model);

            var postcodeValidatorResponse = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode);
            model.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidatorResponse.IsInPilotAreaForOutcome(model.OutcomeGroup);

            if (!model.UserInfo.CurrentAddress.IsInPilotArea)
            {
                ModelState.AddModelError("FindService.CurrentPostcode", "Sorry, this service is not currently available in your area.  Please call NHS 111 for advice now");
                return View(model.CurrentView, model);
            }

            var modelFilterServices = overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices;
            model.DosCheckCapacitySummaryResult = await GetServiceAvailability(model, overrideDate, modelFilterServices, endpoint).ConfigureAwait(false);
            _auditLogger.LogDosResponse(model, model.DosCheckCapacitySummaryResult);

            model.NodeType = NodeType.Outcome;

            if (model.DosCheckCapacitySummaryResult.Error == null &&
                !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                if (model.OutcomeGroup.Is999NonUrgent && !model.DosCheckCapacitySummaryResult.HasITKServices)
                {
                    model.CurrentView = _viewRouter.Build(model, this.ControllerContext).ViewName;
                    return View(model.CurrentView, model);
                }

                model.GroupedDosServices =
                    _dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);

                model = await _outcomeViewModelBuilder.PrimaryCareBuilder(model, reason).ConfigureAwait(false);

                if (model.OutcomeGroup.IsAutomaticSelectionOfItkResult())
                {
                    AutoSelectFirstItkService(model);
                    if (model.SelectedService != null)
                    {
                        var personalDetailsController = DependencyResolver.Current.GetService<PersonalDetailsController>();
                        personalDetailsController.ControllerContext = new ControllerContext(ControllerContext.RequestContext, personalDetailsController);

                        return await personalDetailsController.PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model)).ConfigureAwait(false);
                    }
                }

                if (model.OutcomeGroup.IsUsingRecommendedService || model.OutcomeGroup.IsPrimaryCare)
                {
                    var otherServices =
                        await _recommendedServiceBuilder.BuildRecommendedServicesList(model.DosCheckCapacitySummaryResult.Success.Services).ConfigureAwait(false);
                    var otherServicesModel = Mapper.Map<OtherServicesViewModel>(model);
                    //Very weird mapper issue ignoring this property for some reason
                    //unit test specifically testing this passes fine so can really fathow what is going on
                    //forcing it instead
                    if (otherServicesModel.RecommendedService != null)
                    {
                        otherServicesModel.RecommendedService.ReasonText = model.RecommendedService.ReasonText;
                        otherServicesModel.OtherServices = otherServices.Skip(1);
                    }
                    else
                    {
                        otherServicesModel.OtherServices = otherServices;
                    }

                    return View("~\\Views\\Outcome\\Repeat_Prescription\\RecommendedServiceOtherServices.cshtml", otherServicesModel);
                }

                return View("~\\Views\\Outcome\\ServiceList.cshtml", model);
            }

            if (model.OutcomeGroup.Is999NonUrgent)
            {
                model.CurrentView = _viewRouter.Build(model, this.ControllerContext).ViewName;
            }

            return View(model.OutcomeGroup.IsPharmacyGroup ? "~\\Views\\Outcome\\Repeat_Prescription\\RecommendedServiceNotOffered.cshtml" : model.CurrentView, model);
        }

        private async Task<DosCheckCapacitySummaryResult> GetServiceAvailability(OutcomeViewModel model, DateTime? overrideDate, bool filterServices, DosEndpoint? endpoint)
        {
            var dosViewModel = _dosBuilder.BuildDosViewModel(model, overrideDate);
            _auditLogger.LogDosRequest(model, dosViewModel);
            return await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, filterServices, endpoint).ConfigureAwait(false);
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

            model.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidator.IsInPilotAreaForOutcome(model.OutcomeGroup);

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
            _auditLogger.LogDosRequest(model, dosCase);
            var modelFilterServices = overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices;
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase, modelFilterServices, endpoint).ConfigureAwait(false);
            _auditLogger.LogDosResponse(model, model.DosCheckCapacitySummaryResult);

            if (model.DosCheckCapacitySummaryResult.Error == null &&
                !model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                model.GroupedDosServices =
                    _dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);
                model = await _outcomeViewModelBuilder.ServiceDetailsBuilder(model).ConfigureAwait(false); ;
                if (model.OutcomeGroup.IsAutomaticSelectionOfItkResult())
                {
                    AutoSelectFirstItkService(model);
                    if (model.SelectedService != null)
                    {
                        var personalDetailsController = DependencyResolver.Current.GetService<PersonalDetailsController>();
                        personalDetailsController.ControllerContext = new ControllerContext(ControllerContext.RequestContext, personalDetailsController);

                        return await personalDetailsController.PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model))
                            .ConfigureAwait(false);
                    }
                }
                return View("~\\Views\\Outcome\\ServiceDetails.cshtml", model);
                //explicit path to view because, when direct-linking, the route is no longer /outcome causing convention based view lookup to fail
            }

            return View(model.CurrentView, model);
        }

        [HttpPost]
        public async Task<ActionResult> ReferralExplanation(PersonalDetailViewModel model)
        {
            return View("~\\Views\\Outcome\\Repeat_Prescription\\ReferralExplanation.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> Confirmation(PersonalDetailViewModel model, [FromUri] bool? overrideFilterServices)
        {
            model.OutcomePage = OutcomePage.Confirmation;
            model = await _outcomeViewModelBuilder.ConfirmationSurveyLinkBuilder(model).ConfigureAwait(false);

            var modelFilterServices = overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices;
            var availableServices = await GetServiceAvailability(model, null, modelFilterServices, null).ConfigureAwait(false);
            _auditLogger.LogDosResponse(model, availableServices);
            if (availableServices.ContainsService(model.SelectedService))
            {
                return await SubmitITKDataToService(model).ConfigureAwait(false);
            }

            return ReturnServiceUnavailableView(model, availableServices);
        }

        private ActionResult ReturnServiceUnavailableView(PersonalDetailViewModel model, DosCheckCapacitySummaryResult availableServices)
        {
            var unavailableResult = _referralResultBuilder.BuildServiceUnavailableResult(model, availableServices);
            return View(unavailableResult.ViewName, unavailableResult);
        }

        private async Task<ActionResult> SubmitITKDataToService(PersonalDetailViewModel model)
        {
            var outcomeViewModel = ConvertPatientInformantDateToUserinfo(model.EmailAddress, model);
            var itkConfirmationViewModel = await _outcomeViewModelBuilder.ItkResponseBuilder(outcomeViewModel);
            var result = _referralResultBuilder.Build(itkConfirmationViewModel);
            return View(result.ViewName, result);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmAddress(string longlat, ConfirmLocationViewModel model)
        {
            var results = await _locationResultBuilder.LocationResultByGeouilder(longlat).ConfigureAwait(false);
            var locationResults = Mapper.Map<List<AddressInfoViewModel>>(results.DistinctBy(r => r.Thoroughfare));
            model.FoundLocations = locationResults;
            return View("ConfirmLocation", model);
        }

        private OutcomeViewModel ConvertPatientInformantDateToUserinfo(EmailAddressViewModel emailAddressModel, OutcomeViewModel model)
        {
            model.UserInfo.EmailAddress = emailAddressModel.EmailAddress;
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
        public async Task<ActionResult> EdCallbackAcceptance(PersonalDetailViewModel model, string selectedAnswer)
        {
            model.HasAcceptedCallbackOffer = selectedAnswer.ToLower() == "yes";

            if (model.HasAcceptedCallbackOffer.Value)
            {
                AutoSelectFirstItkService(model);
                if (model.SelectedService != null)
                {
                    var personalDetailsController = DependencyResolver.Current.GetService<PersonalDetailsController>();
                    personalDetailsController.ControllerContext = new ControllerContext(ControllerContext.RequestContext, personalDetailsController);

                    return await personalDetailsController.PersonalDetails(Mapper.Map<PersonalDetailViewModel>(model))
                        .ConfigureAwait(false);
                }
            }

            var outcome = await _outcomeViewModelBuilder.DispositionBuilder(model).ConfigureAwait(false);
            var viewRouter = _viewRouter.Build(outcome, ControllerContext);

            var postcodeValidatorResponse = _postCodeAllowedValidator.IsAllowedPostcode(model.CurrentPostcode);
            model.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidatorResponse.IsInPilotAreaForOutcome(model.OutcomeGroup);

            return View(viewRouter.ViewName, outcome);

        }

        [HttpPost]
        [Route("Outcome/RegisterWithGp", Name = "RegisterWithGp")]
        [Route("Outcome/RegisterWithTempGp", Name = "RegisterWithTempGp")]
        public async Task<ActionResult> MoreInfo(OutcomeViewModel model, string reason)
        {
            _auditLogger.LogPrimaryCareReason(model, reason);
            ViewData["Route"] = ((Route)ControllerContext.RouteData.Route).Url;
            model = await _outcomeViewModelBuilder.PrimaryCareBuilder(model, reason).ConfigureAwait(false);
            return View("~\\Views\\Outcome\\Primary_Care\\MoreInfo.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> SurveyInterstitial(SurveyLinkViewModel model, string fromRoute)
        {
            _auditLogger.LogSurveyInterstitial(model);
            return View("~\\Views\\Outcome\\SurveyInterstitial.cshtml", model);
        }
    }
}

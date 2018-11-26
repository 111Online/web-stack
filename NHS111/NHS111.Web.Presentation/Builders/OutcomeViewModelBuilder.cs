﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Models.Models.Web.Logging;
using NHS111.Utils.Filters;
using NHS111.Utils.Helpers;
using NHS111.Utils.Logging;
using NHS111.Web.Presentation.Logging;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    using System.Diagnostics;
    using NHS111.Models.Mappers.WebMappings;

    public class OutcomeViewModelBuilder : IOutcomeViewModelBuilder
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IJourneyHistoryWrangler _journeyHistoryWrangler;
        private readonly ISurveyLinkViewModelBuilder _surveyLinkViewModelBuilder;
        private readonly IAuditLogger _auditLogger;



        public OutcomeViewModelBuilder(ICareAdviceBuilder careAdviceBuilder, IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, IKeywordCollector keywordCollector,
            IJourneyHistoryWrangler journeyHistoryWrangler, ISurveyLinkViewModelBuilder surveyLinkViewModelBuilder, IAuditLogger auditLogger, IDOSBuilder dosBuilder)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _keywordCollector = keywordCollector;
            _journeyHistoryWrangler = journeyHistoryWrangler;
            _surveyLinkViewModelBuilder = surveyLinkViewModelBuilder;
            _auditLogger = auditLogger;
            _dosBuilder = dosBuilder;
        }

        public async Task<List<AddressInfoViewModel>> SearchPostcodeBuilder(string input)
        {
            input = HttpUtility.UrlDecode(input);
            var listPaf = JsonConvert.DeserializeObject<List<PAF>>(await _restfulHelper.GetAsync(string.Format(_configuration.PostcodeSearchByIdApiUrl, input)));
            return _mappingEngine.Mapper.Map<List<AddressInfoViewModel>>(listPaf);
        }

        public async Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model) {
            var result = await DispositionBuilder(model, null);
            return result;
        }
        public async Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model, DosEndpoint? endpoint)
        {
            model.DispositionTime = DateTime.Now;

            if (OutcomeGroup.Call999Cat2.Equals(model.OutcomeGroup) || OutcomeGroup.Call999Cat3.Equals(model.OutcomeGroup))
            {
                model.CareAdviceMarkers = model.State.Keys.Where(key => key.StartsWith("Cx"));
            }

            Task<SymptomDiscriminator> discriminatorTask = null;
            Task<string> symptomGroupTask = null;
            if (OutcomeGroup.ClinicianCallBack.Equals(model.OutcomeGroup))
            {
                //hard code SD and SG codes to fix DOS for Yorkshire region
                //TODO: Fix this in DOS and remove this hack

                model.SymptomDiscriminatorCode = "4193";
                model.SymptomGroup = "1206";
            } else {
                if (!string.IsNullOrEmpty(model.SymptomDiscriminatorCode)) {
                    discriminatorTask = GetSymptomDiscriminator(model.SymptomDiscriminatorCode);
                }

                var pathways = _journeyHistoryWrangler.GetPathwayNumbers(model.Journey.Steps);

                if (pathways.Any()) {
                    symptomGroupTask = GetSymptomGroup(pathways);
                }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  

                if (discriminatorTask != null) {
                    model.SymptomDiscriminator = await discriminatorTask;
                }
                if (symptomGroupTask != null) {
                    model.SymptomGroup = await symptomGroupTask;
                }
            }

            Task<OutcomeViewModel> dosTask = null;
            if (OutcomeGroup.PrePopulatedDosResultsOutcomeGroups.Contains(model.OutcomeGroup) && !string.IsNullOrEmpty(model.CurrentPostcode)) {
                dosTask = PopulateGroupedDosResults(model, null, null, endpoint);
            }


            Task<CareAdvice> worseningTask = _careAdviceBuilder.FillWorseningCareAdvice(model.UserInfo.Demography.Age, model.UserInfo.Demography.Gender);
            var ageGroup = new AgeCategory(model.UserInfo.Demography.Age).Value;
            var careAdviceKeywords = _keywordCollector.ConsolidateKeywords(model.CollectedKeywords).ToList();
            Task<IEnumerable<CareAdvice>> careAdvicesTask = _careAdviceBuilder.FillCareAdviceBuilder(model.Id, ageGroup, model.UserInfo.Demography.Gender, careAdviceKeywords);
            if (dosTask != null) {
                model = await dosTask;
            }

            Task<SurveyLinkViewModel> surveyTask = _surveyLinkViewModelBuilder.SurveyLinkBuilder(model);
            model.WorseningCareAdvice = await worseningTask;
            model.CareAdvices = await careAdvicesTask;
            model.SurveyLink = await surveyTask;

            return model;
        }

        private bool NeedToRequeryDos(OutcomeViewModel model) {
            return (!model.HasAcceptedCallbackOffer.HasValue || !model.HasAcceptedCallbackOffer.Value) && 
                   model.OutcomeGroup.Equals(OutcomeGroup.AccidentAndEmergency) &&
                   FromOutcomeViewModelToDosViewModel.DispositionResolver.IsRemappedToDx334(model.Id) &&
                   !model.DosCheckCapacitySummaryResult.HasITKServices;
        }

        private async Task<SymptomDiscriminator> GetSymptomDiscriminator(string symptomDiscriminatorCode)
        {

            var symptomDiscriminatorResponse = await
                _restfulHelper.GetResponseAsync(
                    string.Format(_configuration.GetBusinessApiSymptomDiscriminatorUrl(symptomDiscriminatorCode)));
            if (!symptomDiscriminatorResponse.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured getting the symptom discriminator at {0}. {1}",
                    _configuration.GetBusinessApiSymptomDiscriminatorUrl(symptomDiscriminatorCode),
                    await symptomDiscriminatorResponse.Content.ReadAsStringAsync()));

            return 
                JsonConvert.DeserializeObject<SymptomDiscriminator>(await symptomDiscriminatorResponse.Content.ReadAsStringAsync());
        }

        private async Task<string> GetSymptomGroup(string pathways)
        {
            RestfulHelper restfulHelper = new RestfulHelper();

            var symptomGroupResponse = await
                restfulHelper.GetResponseAsync(string.Format(_configuration.GetBusinessApiPathwaySymptomGroupUrl(pathways)));
            if (!symptomGroupResponse.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured getting the symptom group for {0}.", pathways));

            return
                await symptomGroupResponse.Content.ReadAsStringAsync();
        }

        public async Task<OutcomeViewModel> ItkResponseBuilder(OutcomeViewModel model)
        {
            var itkRequestData = CreateItkDispatchRequest(model);
            await _auditLogger.LogItkRequest(model, itkRequestData);
            var response = await SendItkMessage(itkRequestData);
            await _auditLogger.LogItkResponse(model, response);
            model.ItkDuplicate = response.StatusCode == System.Net.HttpStatusCode.Conflict;
            if (response.IsSuccessStatusCode)
            {
                model.ItkSendSuccess = true;
                var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            }
            else
            {
                model.ItkSendSuccess = false;
                Log4Net.Error("Error sending ITK message : Status Code -" + response.StatusCode.ToString() +
                              " Content -" + response.Content.ReadAsStringAsync());
            }
            return model;
        }

        public async Task<OutcomeViewModel> PopulateGroupedDosResults(OutcomeViewModel model, DateTime? overrideDate, bool? overrideFilterServices, DosEndpoint? endpoint) {
            var dosViewModel = _dosBuilder.BuildDosViewModel(model, overrideDate);

            var _ = _auditLogger.LogDosRequest(model, dosViewModel);
            if (!model.HasAcceptedCallbackOffer)
                dosViewModel.Disposition = FromOutcomeViewModelToDosViewModel.DispositionResolver.ConvertToDosCode(originalDx);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            if (NeedToRequeryDos(model)) {
                _auditLogger.LogDosResponse(model);
                dosViewModel.Disposition = FromOutcomeViewModelToDosViewModel.DispositionResolver.ConvertToDosCode(model.Id);
                _auditLogger.LogDosRequest(model, dosViewModel);
                model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            }

            model.DosCheckCapacitySummaryResult.ServicesUnavailable = model.DosCheckCapacitySummaryResult.ResultListEmpty;

            if (!model.DosCheckCapacitySummaryResult.ResultListEmpty)
                model.GroupedDosServices = _dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);

            _surveyLinkViewModelBuilder.AddServiceInformation(model, model.SurveyLink);

             _ = _auditLogger.LogDosResponse(model);
     
            return model;
        }

        public async Task<OutcomeViewModel> DeadEndJumpBuilder(OutcomeViewModel model)
        {
            model.DispositionTime = DateTime.Now;
            model.SurveyLink = await _surveyLinkViewModelBuilder.SurveyLinkBuilder(model);
            return model;
        }

        public async Task<OutcomeViewModel> PathwaySelectionJumpBuilder(OutcomeViewModel model)
        {
            model.DispositionTime = DateTime.Now;
            model.SurveyLink = await _surveyLinkViewModelBuilder.SurveyLinkBuilder(model);
            return model;
        }

        private async Task<HttpResponseMessage> SendItkMessage(ITKDispatchRequest itkRequestData)
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(itkRequestData), Encoding.UTF8, "application/json")
            };
            var response = await _restfulHelper.PostAsync(_configuration.ItkDispatchApiUrl, request);
            return response;
        }

        private Authentication getItkAuthentication()
        {
            return new Authentication { UserName = ConfigurationManager.AppSettings["itk_credential_user"], Password = ConfigurationManager.AppSettings["itk_credential_password"] };
        }

        private ITKDispatchRequest CreateItkDispatchRequest(OutcomeViewModel model)
        {
            var itkRequestData = _mappingEngine.Mapper.Map<OutcomeViewModel, ITKDispatchRequest>(model);
            itkRequestData.Authentication = getItkAuthentication();
            return itkRequestData;
        }

        public async Task<OutcomeViewModel> PersonalDetailsBuilder(OutcomeViewModel model)
        {
            model.CareAdvices = await _careAdviceBuilder.FillCareAdviceBuilder(model.UserInfo.Demography.Age, model.UserInfo.Demography.Gender, model.CareAdviceMarkers.ToList());
            return model;
        }
    }

    public interface IOutcomeViewModelBuilder
    {
        Task<List<AddressInfoViewModel>> SearchPostcodeBuilder(string input);
        Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model,DosEndpoint? endpoint);
        Task<OutcomeViewModel> PersonalDetailsBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> ItkResponseBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> DeadEndJumpBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PathwaySelectionJumpBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PopulateGroupedDosResults(OutcomeViewModel model, DateTime? overrideDate, bool? overrideFilterServices, DosEndpoint? endpoint);
    }
}
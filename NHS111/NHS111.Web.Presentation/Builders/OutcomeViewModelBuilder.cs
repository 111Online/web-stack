using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Utils.Logging;
using NHS111.Utils.Parser;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Logging;
using RestSharp;
using StructureMap.Query;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    using NHS111.Models.Mappers.WebMappings;

    public class OutcomeViewModelBuilder : BaseBuilder, IOutcomeViewModelBuilder
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestClient _restClient;
        private readonly IRestClient _restClientPostcodeApi;
        private readonly IRestClient _restClientItkDispatcherApi;
        private readonly IRestClient _restClientCaseDataCaptureApi;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IJourneyHistoryWrangler _journeyHistoryWrangler;
        private readonly ISurveyLinkViewModelBuilder _surveyLinkViewModelBuilder;
        private readonly IAuditLogger _auditLogger;
        private readonly IRecommendedServiceBuilder _recommendedServiceBuilder;



        public OutcomeViewModelBuilder(ICareAdviceBuilder careAdviceBuilder, IRestClient restClient, IRestClient restClientPostcodeApi, IRestClient restClientItkDispatcherApi, IRestClient restClientCaseDataCaptureApi, IConfiguration configuration, IMappingEngine mappingEngine, IKeywordCollector keywordCollector,
            IJourneyHistoryWrangler journeyHistoryWrangler, ISurveyLinkViewModelBuilder surveyLinkViewModelBuilder, IAuditLogger auditLogger, IDOSBuilder dosBuilder, IRecommendedServiceBuilder recommendedServiceBuilder)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restClient = restClient;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _keywordCollector = keywordCollector;
            _journeyHistoryWrangler = journeyHistoryWrangler;
            _surveyLinkViewModelBuilder = surveyLinkViewModelBuilder;
            _auditLogger = auditLogger;
            _dosBuilder = dosBuilder;
            _restClientPostcodeApi = restClientPostcodeApi;
            _restClientItkDispatcherApi = restClientItkDispatcherApi;
            _restClientCaseDataCaptureApi = restClientCaseDataCaptureApi;
            _recommendedServiceBuilder = recommendedServiceBuilder;
        }

        public async Task<DataCaptureResponse> SendToCaseDataCaptureApi(CaseDataCaptureRequest requestData)
        {
            var request = new JsonRestRequest(_configuration.CaseDataCaptureApiSendSMSMessageUrl, Method.POST);
            request.AddJsonBody(requestData);
            var response = await _restClientCaseDataCaptureApi.ExecuteTaskAsync(request);
            return new DataCaptureResponse(response);
        }

        public async Task<List<AddressInfoViewModel>> SearchPostcodeBuilder(string input)
        {
            input = HttpUtility.UrlDecode(input);
            var url = string.Format(_configuration.PostcodeSearchByIdUrl, input);
            var listPaf = await _restClientPostcodeApi.ExecuteTaskAsync<List<PAF>>(new JsonRestRequest(url, Method.GET));

            CheckResponse(listPaf);

            return _mappingEngine.Mapper.Map<List<AddressInfoViewModel>>(listPaf);
        }

        public async Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model)
        {
            var result = await DispositionBuilder(model, null);
            return result;
        }

        public async Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model, DosEndpoint? endpoint)
        {
            model.DispositionTime = DateTime.Now;

            Task<SymptomDiscriminator> discriminatorTask = null;
            Task<string> symptomGroupTask = null;
            if (model.OutcomeGroup.IsCoronaVirusCallback)
            {
                //hard code SD and SG codes for covidpathway
                //TODO: Fix this in Pathways and remove this hack

                model.SymptomDiscriminatorCode = "8001";
                model.SymptomGroup = "8000";
            }
            else if (OutcomeGroup.ClinicianCallBack.Equals(model.OutcomeGroup))
            {
                //hard code SD and SG codes to fix DOS for Yorkshire region
                //TODO: Fix this in DOS and remove this hack

                model.SymptomDiscriminatorCode = "4193";
                model.SymptomGroup = "1206";
            }
            else
            {
                if (!string.IsNullOrEmpty(model.SymptomDiscriminatorCode))
                {
                    discriminatorTask = GetSymptomDiscriminator(model.SymptomDiscriminatorCode);
                }

                var pathways = _journeyHistoryWrangler.GetPathwayNumbers(model.Journey.Steps);

                if (pathways.Any())
                {
                    symptomGroupTask = GetSymptomGroup(pathways);
                }

                if (discriminatorTask != null)
                {
                    model.SymptomDiscriminator = await discriminatorTask;
                }
                if (symptomGroupTask != null)
                {
                    model.SymptomGroup = await symptomGroupTask;
                }
            }

            var dosTask = Task.FromResult(model);
            if (OutcomeGroup.PrePopulatedDosResultsOutcomeGroups.Contains(model.OutcomeGroup) && !string.IsNullOrEmpty(model.CurrentPostcode))
            {
                dosTask = PopulateGroupedDosResults(model, null, null, endpoint);
            }

            var worseningTask = Task.FromResult(model.WorseningCareAdvice);
            if (!model.WorseningCareAdvice.Items.Any())
                worseningTask = _careAdviceBuilder.FillWorseningCareAdvice(model.UserInfo.Demography.Age, model.UserInfo.Demography.Gender);
            var careAdvicesTask = Task.FromResult(model.CareAdvices);
            if (!model.CareAdvices.Any())
            {
                var ageGroup = new AgeCategory(model.UserInfo.Demography.Age).Value;
                var careAdviceKeywords = _keywordCollector.ConsolidateKeywords(model.CollectedKeywords).ToList();
                careAdvicesTask = _careAdviceBuilder.FillCareAdviceBuilder(model.Id, ageGroup, model.UserInfo.Demography.Gender, careAdviceKeywords);
            }

            model = await dosTask;
            
            if (OutcomeGroup.Call999Cat1.Equals(model.OutcomeGroup) || OutcomeGroup.Call999Cat2.Equals(model.OutcomeGroup) || OutcomeGroup.Call999Cat3.Equals(model.OutcomeGroup))
            {
                model.CareAdviceMarkers = model.State.Keys.Where(key => key.StartsWith("Cx"));
            }

            if (model.Is999Callback)
                model.HasAcceptedCallbackOffer = true;

            var surveyTask = _surveyLinkViewModelBuilder.SurveyLinkBuilder(model);
            model.WorseningCareAdvice = await worseningTask;
            model.CareAdvices = await careAdvicesTask;
            model.SurveyLink = await surveyTask;
            
            return model;
        }

        public SendSmsOutcomeViewModel SendSmsVerifyDetailsBuilder(JourneyViewModel journeyViewModel, string SelectedAnswer)
        {
            var smsSendModel = _mappingEngine.Mapper.Map<SendSmsOutcomeViewModel>(journeyViewModel);
            smsSendModel.Journey = journeyViewModel.Journey;
            smsSendModel.MobileNumber = journeyViewModel.Journey.GetStepInputValue<string>(QuestionType.Telephone, "TX1111");
            smsSendModel.SelectedAnswer = SelectedAnswer;
            return smsSendModel;
        }

        public SendSmsOutcomeViewModel SendSmsDetailsBuilder(JourneyViewModel journeyViewModel)
        {
            //TODO: how to data drive this better?
            var smsSendModel = _mappingEngine.Mapper.Map<SendSmsOutcomeViewModel>(model);
            smsSendModel.MobileNumber = model.Journey.GetStepInputValue<string>(QuestionType.Telephone, "TX1111");

            // If it exists in the state, the age comes from the 111 original journey
            var age = int.Parse(model.State["PATIENT_AGE"]);
            smsSendModel.Age = age >= 0 ? age : model.Journey.GetStepInputValue<int>(QuestionType.Integer, "TX1112");

            smsSendModel.SymptomsStartedDaysAgo = model.Journey.GetStepInputValue<int>(QuestionType.Date, "TX1113");
            smsSendModel.LivesAlone = model.Journey.GetStepInputValue<bool>(QuestionType.Choice, "TX1114");
            smsSendModel.SelectedAnswer = SelectedAnswer;
            return smsSendModel;
        }

        private bool NeedToRequeryDos(OutcomeViewModel model)
        {
            return (!model.HasAcceptedCallbackOffer.HasValue || !model.HasAcceptedCallbackOffer.Value) &&
                   (model.OutcomeGroup.Equals(OutcomeGroup.AccidentAndEmergency) || model.OutcomeGroup.Equals(OutcomeGroup.MentalHealth)) &&
                   FromOutcomeViewModelToDosViewModel.DispositionResolver.IsDOSRetry(model.Id) &&
                   !model.DosCheckCapacitySummaryResult.HasITKServices;
        }

        private async Task<SymptomDiscriminator> GetSymptomDiscriminator(string symptomDiscriminatorCode)
        {
            var url = string.Format(_configuration.GetBusinessApiSymptomDiscriminatorUrl(symptomDiscriminatorCode));
            var symptomDiscriminatorResponse = await _restClient.ExecuteTaskAsync<SymptomDiscriminator>(new JsonRestRequest(url, Method.GET));
            
            if (!symptomDiscriminatorResponse.IsSuccessful)
                throw new Exception(string.Format("A problem occured getting the symptom discriminator at {0}. {1}",
                    _configuration.GetBusinessApiSymptomDiscriminatorUrl(symptomDiscriminatorCode),
                    symptomDiscriminatorResponse.ErrorMessage));

            return symptomDiscriminatorResponse.Data;
        }

        private async Task<string> GetSymptomGroup(string pathways)
        {
            var url = string.Format(_configuration.GetBusinessApiPathwaySymptomGroupUrl(pathways));
            var symptomGroupResponse = await _restClient.ExecuteTaskAsync<string>(new JsonRestRequest(url, Method.GET));

            if (!symptomGroupResponse.IsSuccessful)
                throw new Exception(string.Format("A problem occured getting the symptom group for {0}.", pathways));

            return symptomGroupResponse.Data;
        }

        public async Task<ITKConfirmationViewModel> ItkResponseBuilder(OutcomeViewModel model)
        {
            var itkRequestData = CreateItkDispatchRequest(model);
            _auditLogger.LogItkRequest(model, itkRequestData);
            var response = await SendItkMessage(itkRequestData);
            _auditLogger.LogItkResponse(model, response);
            var itkResponseModel = _mappingEngine.Mapper.Map<OutcomeViewModel, ITKConfirmationViewModel>(model);

            itkResponseModel.ItkDuplicate = IsDuplicateResponse(response);
            itkResponseModel.ItkSendSuccess = response.IsSuccessful;
            if (response.IsSuccessful || IsDuplicateResponse(response))
            {
                itkResponseModel.PatientReference =  response.Content.Replace("\"","");
            }
          
            else
            {
                itkResponseModel.ItkSendSuccess = false;
                Log4Net.Error("Error sending ITK message : Status Code -" + response.StatusCode +
                              " Content -" + response.Content);
            }
            return itkResponseModel;
        }

        private static bool IsDuplicateResponse(IRestResponse response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.Conflict;
        }

        public async Task<OutcomeViewModel> PopulateGroupedDosResults(OutcomeViewModel model, DateTime? overrideDate, bool? overrideFilterServices, DosEndpoint? endpoint)
        {
            var dosViewModel = _dosBuilder.BuildDosViewModel(model, overrideDate);

            _auditLogger.LogDosRequest(model, dosViewModel);
            model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
            if (NeedToRequeryDos(model))
            {
                _auditLogger.LogDosResponse(model, model.DosCheckCapacitySummaryResult);
                dosViewModel.Disposition = FromOutcomeViewModelToDosViewModel.DispositionResolver.ConvertToDosCode(model.Id);
                _auditLogger.LogDosRequest(model, dosViewModel);
                model.DosCheckCapacitySummaryResult = await _dosBuilder.FillCheckCapacitySummaryResult(dosViewModel, overrideFilterServices.HasValue ? overrideFilterServices.Value : model.FilterServices, endpoint);
                model.DosCheckCapacitySummaryResult.IsValidationRequery = true;
            }

            model.DosCheckCapacitySummaryResult.ServicesUnavailable = model.DosCheckCapacitySummaryResult.ResultListEmpty;

            if (!model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                model.GroupedDosServices = _dosBuilder.FillGroupedDosServices(model.DosCheckCapacitySummaryResult.Success.Services);
                model.RecommendedService = await _recommendedServiceBuilder.BuildRecommendedService(model.DosCheckCapacitySummaryResult.Success.FirstService);
            }

            _surveyLinkViewModelBuilder.AddServiceInformation(model, model.SurveyLink);

            _auditLogger.LogDosResponse(model, model.DosCheckCapacitySummaryResult);

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
        
        public async Task<OutcomeViewModel> PrimaryCareBuilder(OutcomeViewModel model, string reason = "")
        {
            model.SurveyLink = await _surveyLinkViewModelBuilder.SurveyLinkBuilder(model);
            _surveyLinkViewModelBuilder.AddDispositionReason(reason, model.SurveyLink);
            return model;
        }
        
        private async Task<IRestResponse> SendItkMessage(ITKDispatchRequest itkRequestData)
        {
            var request = new JsonRestRequest(_configuration.ItkDispatcherApiSendItkMessageUrl, Method.POST);
            request.AddJsonBody(itkRequestData);
            var response = await _restClientItkDispatcherApi.ExecuteTaskAsync(request);
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
        Task<DataCaptureResponse> SendToCaseDataCaptureApi(CaseDataCaptureRequest requestData);
        Task<List<AddressInfoViewModel>> SearchPostcodeBuilder(string input);
        Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model, DosEndpoint? endpoint);
        SendSmsOutcomeViewModel SendSmsDetailsBuilder(JourneyViewModel journeyViewModel);
        SendSmsOutcomeViewModel SendSmsVerifyDetailsBuilder(JourneyViewModel journeyViewModel, string SelectedAnswer);

        Task<OutcomeViewModel> PersonalDetailsBuilder(OutcomeViewModel model);
        Task<ITKConfirmationViewModel> ItkResponseBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> DeadEndJumpBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PathwaySelectionJumpBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PrimaryCareBuilder(OutcomeViewModel model, string reason);
        Task<OutcomeViewModel> PopulateGroupedDosResults(OutcomeViewModel model, DateTime? overrideDate, bool? overrideFilterServices, DosEndpoint? endpoint);
    }
}

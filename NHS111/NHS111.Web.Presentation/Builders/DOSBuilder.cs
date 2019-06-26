using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Models;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;
using NHS111.Features;
using NHS111.Utils.RestTools;
using RestSharp;
using DosService = NHS111.Models.Models.Business.DosService;

namespace NHS111.Web.Presentation.Builders
{
    using Converters;
    using log4net;
    using log4net.Config;
    using NHS111.Models.Mappers.WebMappings;

    public class DOSBuilder : IDOSBuilder
    {
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IITKMessagingFeature _itkMessagingFeature;
        private static readonly ILog _logger = LogManager.GetLogger(typeof (DOSBuilder));

        public DOSBuilder(ICareAdviceBuilder careAdviceBuilder, IRestClient restClient, IConfiguration configuration, IMappingEngine mappingEngine, IITKMessagingFeature itkMessagingFeature)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restClient = restClient;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _itkMessagingFeature = itkMessagingFeature;

            XmlConfigurator.Configure();

        }

        public async Task<DosCheckCapacitySummaryResult> FillCheckCapacitySummaryResult(DosViewModel dosViewModel, bool filterServices, DosEndpoint? endpoint) {

            var checkCapacitySummaryUrl = string.Format("{0}?filterServices={1}&endpoint={2}", _configuration.BusinessDosApiCheckCapacitySummaryUrl, filterServices, endpoint);
            var dosFilterdCase = dosViewModel as DosFilteredCase;

            var request = new JsonRestRequest(checkCapacitySummaryUrl, Method.POST);
            request.AddJsonBody(dosFilterdCase);

            _logger.Debug(string.Format("DOSBuilder.FillCheckCapacitySummaryResult(): URL: {0} BODY: {1}", checkCapacitySummaryUrl, JsonConvert.SerializeObject(dosFilterdCase)));
            var response = await _restClient.ExecuteTaskAsync<DosCheckCapacitySummaryResult>(request);

            if (response.Data.Error != null) return response.Data;

            var services = new List<ServiceViewModel>();
            if (response.Data.Success != null)
            {
                var checkCapacitySummaryResults = JsonConvert.SerializeObject(response.Data.Success.Services);
                var jArray = (JArray) JsonConvert.DeserializeObject(checkCapacitySummaryResults);
                services = jArray.ToObject<List<ServiceViewModel>>();
            }

            var checkCapacitySummaryResult = new DosCheckCapacitySummaryResult()
            {
                Success = new SuccessObject<ServiceViewModel>()
                {
                    Code = (int)response.StatusCode,
                    Services = FilterCallbackEnabled(services)
                }
            };
     
            return checkCapacitySummaryResult;
        }

        public List<GroupedDOSServices> FillGroupedDosServices(List<ServiceViewModel> services)
        {
            var groupedServices = new List<GroupedDOSServices>();
            if (services != null)
            {
                var ungroupedServices = new List<ServiceViewModel>(services);

                while (ungroupedServices.Any())
                {
                    var topServiceType = ungroupedServices.First().OnlineDOSServiceType;
                    groupedServices.Add(new GroupedDOSServices(topServiceType,
                        ungroupedServices.Where(s => s.OnlineDOSServiceType == topServiceType).ToList()));
                    ;
                    ungroupedServices.RemoveAll(s => s.OnlineDOSServiceType == topServiceType);
                }
            }
        return groupedServices;
        }
        
        public List<ServiceViewModel> FilterCallbackEnabled(List<ServiceViewModel> services)
        {
            if (_itkMessagingFeature.IsEnabled)
                return services;
            
            //remove callback services from list, as these are disabled
            services.RemoveAll(s => s.OnlineDOSServiceType.IsReferral);
            return services.ToList();
        }

        public async Task<DosServicesByClinicalTermResult> FillDosServicesByClinicalTermResult(DosViewModel dosViewModel)
        {
            /*
            dosViewModel.SymptomGroup = await BuildSymptomGroup(dosViewModel.JourneyJson);
            
            var request = BuildRequestMessage(dosViewModel);
            var response = await _restfulHelper.PostAsync(_configuration.BusinessDosServicesByClinicalTermUrl, request);

            if (response.StatusCode != HttpStatusCode.OK) return new DosServicesByClinicalTermResult[0];

            var val = await response.Content.ReadAsStringAsync();
            var jObj = (JObject)JsonConvert.DeserializeObject(val);
            var result = jObj["DosServicesByClinicalTermResult"];
            return result.ToObject<DosServicesByClinicalTermResult[]>();
            */

            //USE UNTIL BUSINESS API IS AVAILABLE
            //#########################START###################

            //map doscase to dosservicesbyclinicaltermrequest
            var requestObj = Mapper.Map<DosServicesByClinicalTermRequest>(dosViewModel);
            requestObj.GpPracticeId = await GetPracticeIdFromSurgeryId(dosViewModel.Surgery);

            return
                await
                    GetMobileDoSResponse<DosServicesByClinicalTermResult>(
                        "services/byClinicalTerm/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}",
                        requestObj.CaseId, requestObj.Postcode, requestObj.SearchDistance, requestObj.GpPracticeId,
                        requestObj.Age, requestObj.Gender, requestObj.Disposition,
                        requestObj.SymptomGroupDiscriminatorCombos, requestObj.NumberPerType);

            //################################END################
        }

        public DosViewModel BuildDosViewModel(OutcomeViewModel model, DateTime? overrideDate) {
            var dosViewModel = Mapper.Map<DosViewModel>(model);

            if (model.DosCheckCapacitySummaryResult.IsValidationRequery || (model.HasAcceptedCallbackOffer.HasValue && !model.HasAcceptedCallbackOffer.Value))
                dosViewModel.Disposition = FromOutcomeViewModelToDosViewModel.DispositionResolver.ConvertToDosCode(model.Id);

            if (!overrideDate.HasValue)
            {
                dosViewModel.DispositionTime = DateTime.Now;
            }
            else
            {
                dosViewModel.DispositionTime = overrideDate.Value;
                dosViewModel.SpecifySpecificSearchDate(overrideDate.Value);
            }
            return dosViewModel;
        }

        private async Task<string> GetPracticeIdFromSurgeryId(string surgeryId)
        {
            var services = await GetMobileDoSResponse<DosServicesByClinicalTermResult>("services/byOdsCode/{0}", surgeryId);
            if (services.Success.Code != (int)HttpStatusCode.OK || services.Success.Services.FirstOrDefault() == null) return "0";

            return services.Success.Services.FirstOrDefault().Id.ToString();
        }

        private async Task<T> GetMobileDoSResponse<T>(string endPoint, params object[] args)
        {
            var urlWithRequest = CreateMobileDoSUrl(endPoint, args);
            _logger.Debug("DOSBuilder.FillDosServicesByClinicalTermResult(): URL: " + urlWithRequest);

            var http = new HttpClient(new HttpClientHandler {Credentials = new NetworkCredential(_configuration.DosMobileUsername, _configuration.DosMobilePassword) });
            var response = await http.GetAsync(urlWithRequest);

            var dosResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dosResult);
        }

        private string CreateMobileDoSUrl(string endPoint, params object[] args)
        {
            return string.Format(_configuration.DosMobileBaseUrl + endPoint, args);
        }
    }

    public interface IDOSBuilder
    {
        Task<DosCheckCapacitySummaryResult> FillCheckCapacitySummaryResult(DosViewModel dosViewModel, bool filterServices, DosEndpoint? endpoint);
        Task<DosServicesByClinicalTermResult> FillDosServicesByClinicalTermResult(DosViewModel dosViewModel);
        List<GroupedDOSServices> FillGroupedDosServices(List<ServiceViewModel> services);
        DosViewModel BuildDosViewModel(OutcomeViewModel model, DateTime? overrideDate);
    }
}
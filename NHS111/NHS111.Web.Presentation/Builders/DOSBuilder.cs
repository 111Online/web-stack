using System;
using System.Collections.Generic;
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
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Notifier;
using NHS111.Web.Presentation.Models;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class DOSBuilder : IDOSBuilder
    {
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly ICacheManager<string, string> _cacheManager;
        private readonly INotifier<string> _notifier;
        
        public DOSBuilder(ICareAdviceBuilder careAdviceBuilder, IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, ICacheManager<string, string> cacheManager, INotifier<string> notifier)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _cacheManager = cacheManager;
            _notifier = notifier;
        }

        public async Task<DosCheckCapacitySummaryResult> FillCheckCapacitySummaryResult(DosViewModel dosViewModel)
        {
            if (!string.IsNullOrEmpty(dosViewModel.JourneyJson)) dosViewModel.SymptomGroup = await BuildSymptomGroup(dosViewModel.JourneyJson);
            dosViewModel.SearchDistance = ConvertKilometersToMiles(dosViewModel.SearchDistance);
            var request = BuildRequestMessage(dosViewModel);
            var response = await _restfulHelper.PostAsync(_configuration.BusinessDosCheckCapacitySummaryUrl, request);

            if (response.StatusCode != HttpStatusCode.OK) return new DosCheckCapacitySummaryResult { Error = new ErrorObject() { Code = (int) response.StatusCode, Message = response.ReasonPhrase } };

            var val = await response.Content.ReadAsStringAsync();
            var jObj = (JObject)JsonConvert.DeserializeObject(val);
            var result = jObj["CheckCapacitySummaryResult"];
            var checkCapacitySummaryResult = new DosCheckCapacitySummaryResult()
            {
                Success = new SuccessObject<DosService>()
                {
                    Code = (int)response.StatusCode,
                    Services = result.ToObject<List<DosService>>()
                }
            };
            
            return checkCapacitySummaryResult;
        }
        
        private int ConvertKilometersToMiles(int metricSearchDistance) {
            const float MILES_PER_KM = 1.609344f;
            float miles = metricSearchDistance / MILES_PER_KM;
            return (int)Math.Ceiling(miles);
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

        public async Task<DosViewModel> FillServiceDetailsBuilder(DosViewModel model)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(model.CheckCapacitySummaryResultListJson);
            model.DosCheckCapacitySummaryResult = jObj["DosCheckCapacitySummaryResult"].ToObject<DosCheckCapacitySummaryResult>();
            var selectedService = model.SelectedService;

            var itkMessage = await _cacheManager.Read(model.UserId.ToString());
            var document = XDocument.Parse(itkMessage);

            var serviceDetials = document.Root.Descendants("ServiceDetails").FirstOrDefault();
            serviceDetials.Element("id").SetValue(selectedService.Id.ToString());
            serviceDetials.Element("name").SetValue(selectedService.Name);
            serviceDetials.Element("odsCode").SetValue(selectedService.OdsCode);
            serviceDetials.Element("contactDetails").SetValue(selectedService.ContactDetails ?? "");
            serviceDetials.Element("address").SetValue(selectedService.Address);
            serviceDetials.Element("postcode").SetValue(selectedService.PostCode);

            _cacheManager.Set(model.UserId.ToString(), document.ToString());
            _notifier.Notify(_configuration.IntegrationApiItkDispatcher, model.UserId.ToString());

            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult { Success = new SuccessObject<DosService>() { Services = new List<DosService>() { selectedService } } };
            model.CareAdvices = await _careAdviceBuilder.FillCareAdviceBuilder(Convert.ToInt32(model.Age), model.Gender.ToString(), model.CareAdviceMarkers.ToList());

            return model;
        }

        public async Task<int> BuildSymptomGroup(string journeyJson)
        {
            var journey = JsonConvert.DeserializeObject<Journey>(journeyJson);
            var symptomGroup =
                await
                    _restfulHelper.GetAsync(
                        _configuration.GetBusinessApiPathwaySymptomGroupUrl(string.Join(",",
                            journey.Steps.Select(s => s.QuestionId.Split('.').First()).Distinct())));
            return int.Parse(symptomGroup);
        }

        public HttpRequestMessage BuildRequestMessage(DosCase dosCase)
        {
            var dosCheckCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(_configuration.DosUsername, _configuration.DosPassword, dosCase);
            return new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(dosCheckCapacitySummaryRequest), Encoding.UTF8, "application/json") };
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

            var http = new HttpClient(new HttpClientHandler {Credentials = new NetworkCredential(_configuration.DOSMobileUsername, _configuration.DOSMobilePassword) });
            var response = await http.GetAsync(urlWithRequest);

            var dosResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dosResult);
        }

        private string CreateMobileDoSUrl(string endPoint, params object[] args)
        {
            return string.Format(_configuration.DOSMobileBaseUrl + endPoint, args);
        }
    }

    public interface IDOSBuilder
    {
        Task<DosCheckCapacitySummaryResult> FillCheckCapacitySummaryResult(DosViewModel dosViewModel);
        Task<DosServicesByClinicalTermResult> FillDosServicesByClinicalTermResult(DosViewModel dosViewModel);
        Task<DosViewModel> FillServiceDetailsBuilder(DosViewModel model);
        Task<int> BuildSymptomGroup(string journeyJson);
        HttpRequestMessage BuildRequestMessage(DosCase dosCase);
    }
}
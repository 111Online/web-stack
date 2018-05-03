using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Business.DOS.WhitelistFilter;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Features;
using NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.Service
{
    public class ServiceAvailabilityFilterService : IServiceAvailabilityFilterService
    {
        private readonly IDosService _dosService;
        private readonly IConfiguration _configuration;
        private readonly IServiceAvailabilityManager _serviceAvailabilityManager;
        private readonly IFilterServicesFeature _filterServicesFeature;
        private readonly IServiceWhitelistFilter _serviceWhitelistFilter;
        private readonly IOnlineServiceTypeMapper _serviceTypeMapper;
        private readonly IOnlineServiceTypeFilter _serviceTypeFilter;

        public ServiceAvailabilityFilterService(IDosService dosService, IConfiguration configuration, IServiceAvailabilityManager serviceAvailabilityManager, IFilterServicesFeature filterServicesFeature, IServiceWhitelistFilter serviceWhitelistFilter, IOnlineServiceTypeMapper serviceTypeMapper, IOnlineServiceTypeFilter serviceTypeFilter)
        {
            _dosService = dosService;
            _configuration = configuration;
            _serviceAvailabilityManager = serviceAvailabilityManager;
            _filterServicesFeature = filterServicesFeature;
            _serviceWhitelistFilter = serviceWhitelistFilter;
            _serviceTypeMapper = serviceTypeMapper;
            _serviceTypeFilter = serviceTypeFilter;
        }

        public async Task<HttpResponseMessage> GetFilteredServices(HttpRequestMessage request, bool filterServices, DosEndpoint? endpoint)
        {
            var content = await request.Content.ReadAsStringAsync();

            var dosCase = GetObjectFromRequest<DosCase>(content);
            dosCase.SearchDistance = _configuration.DoSSearchDistance;

            var dosCaseRequest = BuildRequestMessage(dosCase);
            var originalPostcode = dosCase.PostCode;

            var response = await _dosService.GetServices(dosCaseRequest, endpoint);

            if (response.StatusCode != HttpStatusCode.OK) return response;

            var dosFilteredCase = GetObjectFromRequest<DosFilteredCase>(content);

            var val = await response.Content.ReadAsStringAsync();
            var jObj = (JObject)JsonConvert.DeserializeObject(val);
            var services = jObj["CheckCapacitySummaryResult"];
            var results = services.ToObject<List<Models.Models.Business.DosService>>();


            var filteredByServiceWhitelistResults = await _serviceWhitelistFilter.Filter(results, originalPostcode);
            var mappedByServiceTypeResults = await _serviceTypeMapper.Map(filteredByServiceWhitelistResults, originalPostcode);
            var filteredByUnknownTypeResults = _serviceTypeFilter.FilterUnknownTypes(mappedByServiceTypeResults);
        
            if (!_filterServicesFeature.IsEnabled && !filterServices)
            {
                return BuildResponseMessage(filteredByUnknownTypeResults);
            }
            var filteredByclosedCallbackTypeResults = _serviceTypeFilter.FilterClosedCallbackServices(filteredByUnknownTypeResults);
            var serviceAvailability = _serviceAvailabilityManager.FindServiceAvailability(dosFilteredCase);
            return BuildResponseMessage(serviceAvailability.Filter(filteredByclosedCallbackTypeResults));
        }


        public HttpRequestMessage BuildRequestMessage(DosCase dosCase)
        {
            var dosCheckCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(_configuration.DosUsername, _configuration.DosPassword, dosCase);
            return new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(dosCheckCapacitySummaryRequest), Encoding.UTF8, "application/json") };
        }

        public HttpResponseMessage BuildResponseMessage(IEnumerable<Models.Models.Business.DosService> results)
        {
            var result = new JsonCheckCapacitySummaryResult(results);
            return new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json") };
        }

        public T GetObjectFromRequest<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }

    public interface IServiceAvailabilityFilterService
    {
        Task<HttpResponseMessage> GetFilteredServices(HttpRequestMessage request, bool filterServices, DosEndpoint? endpoint);
    }

    public class JsonCheckCapacitySummaryResult
    {
        private readonly CheckCapacitySummaryResult[] _checkCapacitySummaryResults;

        public JsonCheckCapacitySummaryResult(IEnumerable<Models.Models.Business.DosService> dosServices)
        {
            var serialisedServices = JsonConvert.SerializeObject(dosServices);
            _checkCapacitySummaryResults = JsonConvert.DeserializeObject<CheckCapacitySummaryResult[]>(serialisedServices);
        }

        [JsonProperty(PropertyName = "CheckCapacitySummaryResult")]
        public CheckCapacitySummaryResult[] CheckCapacitySummaryResults
        {
            get { return _checkCapacitySummaryResults; }
        }
    }
}

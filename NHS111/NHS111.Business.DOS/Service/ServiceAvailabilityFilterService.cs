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
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Models;
using NHS111.Features;

namespace NHS111.Business.DOS.Service
{
    public class ServiceAvailabilityFilterService : IServiceAvailabilityFilterService
    {
        private readonly IDosService _dosService;
        private readonly IConfiguration _configuration;
        private readonly IServiceAvailabilityManager _serviceAvailabilityManager;
        private readonly IFilterServicesFeature _filterServicesFeature;

        public ServiceAvailabilityFilterService(IDosService dosService, IConfiguration configuration, IServiceAvailabilityManager serviceAvailabilityManager, IFilterServicesFeature filterServicesFeature)
        {
            _dosService = dosService;
            _configuration = configuration;
            _serviceAvailabilityManager = serviceAvailabilityManager;
            _filterServicesFeature = filterServicesFeature;
        }

        public async Task<HttpResponseMessage> GetFilteredServices(HttpRequestMessage request, bool filterServices)
        {
            var content = await request.Content.ReadAsStringAsync();
            var dosCaseRequest = BuildRequestMessage(GetObjectFromRequest<DosCase>(content));
            var response = await _dosService.GetServices(dosCaseRequest);

            if (response.StatusCode != HttpStatusCode.OK) return response;

            var dosFilteredCase = GetObjectFromRequest<DosFilteredCase>(content);

            var val = await response.Content.ReadAsStringAsync();
            var jObj = (JObject)JsonConvert.DeserializeObject(val);
            var services = jObj["CheckCapacitySummaryResult"];
            var results = services.ToObject<List<Models.Models.Web.FromExternalServices.DosService>>();

            if (!_filterServicesFeature.IsEnabled && !filterServices) return BuildResponseMessage(results);

            var serviceAvailability = _serviceAvailabilityManager.FindServiceAvailability(dosFilteredCase);
            return BuildResponseMessage(serviceAvailability.Filter(results));
        }


        public HttpRequestMessage BuildRequestMessage(DosCase dosCase)
        {
            var dosCheckCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(_configuration.DosUsername, _configuration.DosPassword, dosCase);
            return new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(dosCheckCapacitySummaryRequest), Encoding.UTF8, "application/json") };
        }

        public HttpResponseMessage BuildResponseMessage(IEnumerable<Models.Models.Web.FromExternalServices.DosService> results)
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
        Task<HttpResponseMessage> GetFilteredServices(HttpRequestMessage request, bool filterServices);
    }

    public class JsonCheckCapacitySummaryResult
    {
        private readonly CheckCapacitySummaryResult[] _checkCapacitySummaryResults;

        public JsonCheckCapacitySummaryResult(IEnumerable<Models.Models.Web.FromExternalServices.DosService> dosServices)
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

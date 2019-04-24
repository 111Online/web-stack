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
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Converters;
using CheckCapacitySummaryResult = NHS111.Models.Models.Business.CheckCapacitySummaryResult;

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
        private readonly IPublicHolidayService _publicHolidayService;
        private readonly ISearchDistanceService _searchDistanceService;

        public ServiceAvailabilityFilterService(IDosService dosService, IConfiguration configuration, IServiceAvailabilityManager serviceAvailabilityManager, IFilterServicesFeature filterServicesFeature, IServiceWhitelistFilter serviceWhitelistFilter, IOnlineServiceTypeMapper serviceTypeMapper, IOnlineServiceTypeFilter serviceTypeFilter, IPublicHolidayService publicHolidayService, ISearchDistanceService searchDistanceService)
        {
            _dosService = dosService;
            _configuration = configuration;
            _serviceAvailabilityManager = serviceAvailabilityManager;
            _filterServicesFeature = filterServicesFeature;
            _serviceWhitelistFilter = serviceWhitelistFilter;
            _serviceTypeMapper = serviceTypeMapper;
            _serviceTypeFilter = serviceTypeFilter;
            _publicHolidayService = publicHolidayService;
            _searchDistanceService = searchDistanceService;
        }

        public async Task<DosCheckCapacitySummaryResult> GetFilteredServices(DosFilteredCase dosFilteredCase, bool filterServices, DosEndpoint? endpoint)
        {
            dosFilteredCase.SearchDistance = await _searchDistanceService.GetSearchDistanceByPostcode(dosFilteredCase.PostCode);

            var dosCaseRequest = BuildRequest(dosFilteredCase);
            var originalPostcode = dosFilteredCase.PostCode;
            var result = await _dosService.GetServices(dosCaseRequest, endpoint);

            if (result.Error != null) return result;

            var checkCapacitySummaryResults = JsonConvert.SerializeObject(result.Success.Services);
            var jArray = (JArray)JsonConvert.DeserializeObject(checkCapacitySummaryResults);
            
            // get the search datetime if one has been set, if not set to now
            DateTime searchDateTime;
            if (!DateTime.TryParse(dosFilteredCase.SearchDateTime, out searchDateTime)) 
                searchDateTime = DateTime.Now;

            // use dosserviceconvertor to specify the time to use for each dos service object
            var settings = new JsonSerializer();
            settings.Converters.Add(new DosServiceConverter(new SearchDateTimeClock(searchDateTime)));
            var results = jArray.ToObject<IList<Models.Models.Business.DosService>>(settings);

            var publicHolidayAjustedResults =
                _publicHolidayService.AdjustServiceRotaSessionOpeningForPublicHoliday(results);

            var filteredByServiceWhitelistResults = await _serviceWhitelistFilter.Filter(publicHolidayAjustedResults.ToList(), originalPostcode);
            var mappedByServiceTypeResults = await _serviceTypeMapper.Map(filteredByServiceWhitelistResults, originalPostcode);
            var filteredByUnknownTypeResults = _serviceTypeFilter.FilterUnknownTypes(mappedByServiceTypeResults);
        
            if (!_filterServicesFeature.IsEnabled && !filterServices)
            {
                return _dosService.BuildDosCheckCapacitySummaryResult(filteredByUnknownTypeResults);
            }
            var filteredByclosedCallbackTypeResults = _serviceTypeFilter.FilterClosedCallbackServices(filteredByUnknownTypeResults);
            var serviceAvailability = _serviceAvailabilityManager.FindServiceAvailability(dosFilteredCase);
            return _dosService.BuildDosCheckCapacitySummaryResult(serviceAvailability.Filter(filteredByclosedCallbackTypeResults));
        }


        public DosCheckCapacitySummaryRequest BuildRequest(DosCase dosCase)
        {
            var dosCheckCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(_configuration.DosUsername, _configuration.DosPassword, dosCase);
            return dosCheckCapacitySummaryRequest;
        }

        public T GetObjectFromRequest<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }

    public interface IServiceAvailabilityFilterService
    {
        Task<DosCheckCapacitySummaryResult> GetFilteredServices(DosFilteredCase dosCase, bool filterServices, DosEndpoint? endpoint);
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

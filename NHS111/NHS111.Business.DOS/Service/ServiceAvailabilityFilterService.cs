using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Models;

namespace NHS111.Business.DOS.Service
{
    public class ServiceAvailabilityFilterService : IServiceAvailabilityFilterService
    {
        private readonly IDosService _dosService;
        private readonly IConfiguration _configuration;

        public ServiceAvailabilityFilterService(IDosService dosService, IConfiguration configuration)
        {
            _dosService = dosService;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> GetFilteredServices(HttpRequestMessage request)
        {
            var dosFilteredCase = GetObjectFromRequest<DosFilteredCase>(await request.Content.ReadAsStringAsync());
            var dosCaseRequest = BuildRequestMessage(dosFilteredCase);
            var response = await _dosService.GetServices(dosCaseRequest);

            if (response.StatusCode != HttpStatusCode.OK) return response;
            
            var isFiltered = GetFilterDosResults(dosFilteredCase.Disposition);
            if (!isFiltered) return response;

            var result = GetObjectFromRequest<DosCheckCapacitySummaryResult>(await response.Content.ReadAsStringAsync());

            var serviceAvailability = new ServiceAvailability(new ServiceAvailabilityProfile(new ProfileHoursOfOperation(_configuration)), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (!serviceAvailability.IsOutOfHours)
                result.Success.Services = result.Success.Services
                    .Where(s => FilteredDosServiceIds.Contains((int)s.ServiceType.Id))
                    .ToList();

            var responseMessage = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(result)) };
            return responseMessage;
        }

        private bool GetFilterDosResults(int dispositionCode)
        {
            var filteredDispositionCodes = _configuration.FilteredDispositionCodes;
            return !string.IsNullOrEmpty(filteredDispositionCodes) && filteredDispositionCodes.Split('|').Select(c => Convert.ToInt32(c)).Contains(dispositionCode);

        }

        public IEnumerable<int> FilteredDosServiceIds
        {
            get
            {
                var filteredDosServiceIds = _configuration.FilteredDosServiceIds;
                return !string.IsNullOrEmpty(filteredDosServiceIds) ? filteredDosServiceIds.Split('|').Select(c => Convert.ToInt32(c)) : new List<int>();
            }
        }

        public HttpRequestMessage BuildRequestMessage(DosCase dosCase)
        {
            var dosCheckCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(_configuration.DosUsername, _configuration.DosPassword, dosCase);
            return new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(dosCheckCapacitySummaryRequest), Encoding.UTF8, "application/json") };
        }

        public T GetObjectFromRequest<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }

    public interface IServiceAvailabilityFilterService
    {
        Task<HttpResponseMessage> GetFilteredServices(HttpRequestMessage request);

        IEnumerable<int> FilteredDosServiceIds { get; }
    }
}

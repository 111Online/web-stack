using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Models;

namespace NHS111.Business.DOS.Service
{
    public class ServiceAvailabilityFilterService : IServiceAvailabilityFilterService
    {
        private readonly IDosService _dosService;
        private readonly List<long> _filteredServiceTypes = new List<long>() {1, 2, 3};

        public ServiceAvailabilityFilterService(IDosService dosService)
        {
            _dosService = dosService;
        }

        public async Task<HttpResponseMessage> GetFilteredServices(bool isFiltered, HttpRequestMessage request)
        {
            var response = await _dosService.GetServices(request);

            if (!isFiltered) return response;

            if (response.StatusCode != HttpStatusCode.OK) return response;


            var requestContent = await request.Content.ReadAsStringAsync();
            var dosCase = JsonConvert.DeserializeObject<DosCase>(requestContent);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DosCheckCapacitySummaryResult>(content);

            //need to figure out time frame and dispo time
            //then call service availability
            //dosCase.Time?
            //dosCase.TimeFrame?  
            //will this break dos?
            var dispositonTime = DateTime.Now;
            var serviceAvailability = new ServiceAvailability(new ServiceAvailabilityProfile(new ProfileHoursOfOperation(new Configuration.Configuration())), dispositonTime, 0 );
            if (!serviceAvailability.IsOutOfHours)
                result.Success.Services = result.Success.Services
                    .Where(s => _filteredServiceTypes.Contains(s.ServiceType.Id))
                    .ToList();

            var responseMessage = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(result)) };

            return responseMessage;
        }
    }

    public interface IServiceAvailabilityFilterService
    {
        Task<HttpResponseMessage> GetFilteredServices(bool isFiltered, HttpRequestMessage request);
    }
}

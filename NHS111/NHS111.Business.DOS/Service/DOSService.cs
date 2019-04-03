using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.DOS.Service
{
    using System.Web;
    using Models.Models.Web.DosRequests;

    public class DosService : IDosService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;

        public DosService(IConfiguration configuration, IRestClient restClientDosDomainApi)
        {
            _configuration = configuration;
            _restClient = restClientDosDomainApi;
        }
        public async Task<IRestResponse<IEnumerable<CheckCapacitySummaryResult>>> GetServices(DosCheckCapacitySummaryRequest dosRequest, DosEndpoint? endpoint) {

            var url = string.Format("{0}?endpoint={1}", _configuration.DomainDosApiCheckCapacitySummaryUrl, !endpoint.HasValue ? DosEndpoint.Unspecified : endpoint.Value);
            var request = new JsonRestRequest(url, Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(dosRequest));

            return await _restClient.ExecuteTaskAsync<IEnumerable<CheckCapacitySummaryResult>>(request);
        }

        public async Task<ServiceDetailsByIdResponse> GetServiceById(DosServiceDetailsByIdRequest serviceDetailsByIdRequest)
        {
            var request = new JsonRestRequest(_configuration.DomainDosApiServiceDetailsByIdUrl, Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(serviceDetailsByIdRequest));

            var response = await _restClient.ExecuteTaskAsync<ServiceDetailsByIdResponse>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data;
            throw response.ErrorException;
        }
    }

    public interface IDosService
    {
        Task<IRestResponse<IEnumerable<CheckCapacitySummaryResult>>> GetServices(DosCheckCapacitySummaryRequest dosRequest, DosEndpoint? endpoint);

        Task<ServiceDetailsByIdResponse> GetServiceById(DosServiceDetailsByIdRequest serviceDetailsByIdRequest);
    }
}

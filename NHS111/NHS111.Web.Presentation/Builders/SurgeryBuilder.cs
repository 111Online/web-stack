using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    public class SurgeryBuilder : ISurgeryBuilder
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public SurgeryBuilder(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public Task<List<Surgery>> SearchSurgeryBuilder(string input)
        {
            throw new System.NotImplementedException();
        }

        public Task<Surgery> SurgeryByIdBuilder(string surgeryId)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ISurgeryBuilder
    {
        Task<List<Surgery>> SearchSurgeryBuilder(string input);

        Task<Surgery> SurgeryByIdBuilder(string surgeryId);
    }
}

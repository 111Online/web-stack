using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHS111.Utils.RestTools;

namespace NHS111.Web.Presentation.Builders
{
    public class SurgeryBuilder : ISurgeryBuilder
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;

        public SurgeryBuilder(ILoggingRestClient restClient, IConfiguration configuration)
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

using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHS111.Web.Presentation.Builders
{
    public class SurgeryBuilder : ISurgeryBuilder
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public SurgeryBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<List<Surgery>> SearchSurgeryBuilder(string input)
        {
            if (string.IsNullOrEmpty(input)) return new List<Surgery>();

            var surgeriers = JsonConvert.DeserializeObject<List<Surgery>>(await _restfulHelper.GetAsync(string.Format(_configuration.GPSearchApiUrl, input)));
            return surgeriers;
        }

        public async Task<Surgery> SurgeryByIdBuilder(string surgeryId)
        {
            return string.IsNullOrEmpty(surgeryId) ? new Surgery() { SurgeryId = "UKN" } : JsonConvert.DeserializeObject<Surgery>(await _restfulHelper.GetAsync(string.Format(_configuration.GPSearchByIdUrl, surgeryId)));
        }
    }

    public interface ISurgeryBuilder
    {
        Task<List<Surgery>> SearchSurgeryBuilder(string input);

        Task<Surgery> SurgeryByIdBuilder(string surgeryId);
    }
}

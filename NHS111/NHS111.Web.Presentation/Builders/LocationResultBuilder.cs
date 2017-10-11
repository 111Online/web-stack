using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Presentation.Builders
{
    public class LocationResultBuilder : ILocationResultBuilder
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private const string SubscriptionKey = "Ocp-Apim-Subscription-Key";

        public LocationResultBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<List<LocationResult>> LocationResultByPostCodeBuilder(string postCode)
        {
            if (string.IsNullOrEmpty(postCode)) return new List<LocationResult>();
            var headers = new Dictionary<string, string>() {{ SubscriptionKey, _configuration.PostcodeSubscriptionKey} };
            var locationResults = JsonConvert.DeserializeObject<List<LocationResult>>(await _restfulHelper.GetAsync(string.Format(_configuration.PostcodeSearchByIdApiUrl, postCode), headers));
            return locationResults;
        }
    }

    public interface ILocationResultBuilder
    {
        Task<List<LocationResult>> LocationResultByPostCodeBuilder(string postCode);
    }
}
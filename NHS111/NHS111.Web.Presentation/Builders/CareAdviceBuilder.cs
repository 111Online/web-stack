using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Presentation.Builders
{
    public class CareAdviceBuilder : ICareAdviceBuilder
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public CareAdviceBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(int age, string gender, IList<string> careAdviceMarkers)
        {
            var careAdvices = careAdviceMarkers.Any()
                 ? JsonConvert.DeserializeObject<List<CareAdvice>>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiCareAdviceUrl(age, gender, string.Join(",", careAdviceMarkers))))
                 : Enumerable.Empty<CareAdvice>();

            return careAdvices;
        }
    }

    public interface ICareAdviceBuilder
    {
        Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(int age, string gender, IList<string> careAdviceMarkers);

    }
}
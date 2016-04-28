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

        public async Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(string dxCode, string ageGroup, string gender, IList<string> careAdviceKeywords)
        {
            var careAdvices = careAdviceKeywords.Any()
                 ? JsonConvert.DeserializeObject<List<CareAdvice>>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiInterimCareAdviceUrl(dxCode, ageGroup, gender, GenerateKeywordsList(careAdviceKeywords))))
                 : Enumerable.Empty<CareAdvice>();

            return careAdvices;
        }

        private string GenerateKeywordsList(IList<string> careAdviceKeywords)
        {
            return careAdviceKeywords.Aggregate((i, j) => i + '|' + j);
        }
    }

    public interface ICareAdviceBuilder
    {
        Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(int age, string gender, IList<string> careAdviceMarkers);

        Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(string dxCode, string ageGroup, string gender,
            IList<string> careAdviceKeywords);

    }
}
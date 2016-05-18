using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            if(!careAdviceKeywords.Any()) return Enumerable.Empty<CareAdvice>();

            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(careAdviceKeywords), Encoding.UTF8, "application/json") };
            var response = await (await _restfulHelper.PostAsync(_configuration.GetBusinessApiInterimCareAdviceUrl(dxCode, ageGroup, gender), request)).Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<CareAdvice>>(response);
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


using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    using Configuration;
    using NHS111.Models.Models.Domain;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CareAdviceBuilder
        : BaseBuilder, ICareAdviceBuilder
    {

        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private const string WORSENING_CAREADVICE_ID = "CX1910";

        public CareAdviceBuilder(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(int age, string gender, IList<string> careAdviceMarkers)
        {
            if (!careAdviceMarkers.Any())
                return Enumerable.Empty<CareAdvice>();

            var businessApiCareAdviceUrl = _configuration.GetBusinessApiCareAdviceUrl(age, gender, string.Join(",", careAdviceMarkers));
            var careAdvices = await _restClient.ExecuteTaskAsync<IEnumerable<CareAdvice>>(new JsonRestRequest(businessApiCareAdviceUrl, Method.GET));

            CheckResponse(careAdvices);

            return careAdvices.Data;
        }

        public async Task<CareAdvice> FillWorseningCareAdvice(int age, string gender)
        {

            var businessApiCareAdviceUrl = _configuration.GetBusinessApiCareAdviceUrl(age, gender, WORSENING_CAREADVICE_ID);
            var careAdvices = await _restClient.ExecuteTaskAsync<IEnumerable<CareAdvice>>(new JsonRestRequest(businessApiCareAdviceUrl, Method.GET));

            CheckResponse(careAdvices);

            return careAdvices.Data.FirstOrDefault();
        }

        public async Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(string dxCode, string ageGroup, string gender, IList<string> careAdviceKeywords)
        {
            if (!careAdviceKeywords.Any())
                return Enumerable.Empty<CareAdvice>();

            var businessApiInterimCareAdviceUrl = _configuration.GetBusinessApiInterimCareAdviceUrl(dxCode, ageGroup, gender);
            var request = new JsonRestRequest(businessApiInterimCareAdviceUrl, Method.POST);
            request.AddJsonBody(GenerateKeywordsList(careAdviceKeywords));
            var careAdvices = await _restClient.ExecuteTaskAsync<IEnumerable<CareAdvice>>(request);

            CheckResponse(careAdvices);

            return careAdvices.Data;
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

        Task<CareAdvice> FillWorseningCareAdvice(int age, string gender);

    }
}
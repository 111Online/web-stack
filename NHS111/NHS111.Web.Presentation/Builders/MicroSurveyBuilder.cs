using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    public class MicroSurveyBuilder
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;

        public MicroSurveyBuilder(ILoggingRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<string> GetMicroSurveyQuestion(string questionId)
        {
            var uri = string.Format("API/v3/survey-definitions/{0}/questions/{1}", _configuration.QualtricsRecommendedServiceSurveyId, questionId);

            var request = new RestRequest(_configuration.QualtricsApiBaseUrl, Method.GET);

            request.AddHeader("X-API-TOKEN", _configuration.QualtricsApiToken);

            var result = await _restClient.ExecuteAsync<string>(request);

            return result.Content;
        }

        public async Task PostMicroSurveyResponse(string )
        {

        }
    }
}

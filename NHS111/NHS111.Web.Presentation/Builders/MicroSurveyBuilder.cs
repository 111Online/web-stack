using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.MicroSurvey;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    public class MicroSurveyBuilder : IMicroSurveyBuilder
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;

        public MicroSurveyBuilder(ILoggingRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task PostMicroSurveyResponse(SurveyResult surveyResult)
        {
            var uri = string.Format("API/v3/surveys/{0}/responses", _configuration.QualtricsRecommendedServiceSurveyId);

            var request = new RestRequest(uri, Method.POST);

            request.AddHeader("X-API-TOKEN", _configuration.QualtricsApiToken);

            request.AddParameter("application/json", JsonConvert.SerializeObject(new
            {
                values = JsonConvert.DeserializeObject(surveyResult.Values)
            }), ParameterType.RequestBody);

           var result = await _restClient.ExecuteAsync(request);
        }
    }

    public interface IMicroSurveyBuilder
    {
        Task PostMicroSurveyResponse(SurveyResult json);
    }
}

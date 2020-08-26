using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Azure.Core;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.MicroSurvey;
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
            surveyResult.Values = AddUrlProperty(surveyResult.Values);

            request.AddParameter("application/json", JsonConvert.SerializeObject(new
            {
                values = JsonConvert.DeserializeObject(surveyResult.Values)
            }), ParameterType.RequestBody);

           var result = await _restClient.ExecuteAsync(request);
        }

        private string AddUrlProperty(string values)
        {
            var embeddedData = JsonConvert.DeserializeObject<EmbeddedData>(values);
            if (embeddedData == null) return values;

            embeddedData.QURL =  string.Format("{0}API/v3/surveys/{1}/responses", _configuration.QualtricsApiBaseUrl, _configuration.QualtricsRecommendedServiceSurveyId);
            return JsonConvert.SerializeObject(embeddedData);
        }
    }

    public interface IMicroSurveyBuilder
    {
        Task PostMicroSurveyResponse(SurveyResult json);
    }
}

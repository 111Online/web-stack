
namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using Models.Models.Web.DosRequests;
    using Models.Models.Web.ITK;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using RestSharp;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class TestBench
    {

        public TestBench()
        {
            _client = new RestClient("http://localhost:55954/");
        }

        public IDosTestScenarioSetup SetupDosScenario()
        {
            return new DosTestScenarioSetup();
        }

        public IEsbTestScenarioSetup SetupEsbScenario()
        {
            return new EsbTestScenarioSetup();
        }

        public async Task<VerificationResult> Verify(IDosTestScenario scenario)
        {
            var request = new VerifyDosTestScenarioPostRequest(scenario.Postcode);
            var response = await _client.ExecutePostTaskAsync(request);
            VerificationResult result;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return JsonConvert.DeserializeObject<SuccessfulVerificationResult>(response.Content);
                case HttpStatusCode.NotFound:
                    result = JsonConvert.DeserializeObject<NoMatchingScenarioVerificationResult>(response.Content);
                    break;
                case HttpStatusCode.Conflict:
                    result = JsonConvert.DeserializeObject<IncorrectNumberOfRequestsVerificationResult<DosFilteredCase>>(response.Content);
                    break;
                case HttpStatusCode.BadRequest:
                    result = JsonConvert.DeserializeObject<RequestMismatchVerificationResult<DosFilteredCase>>(response.Content);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Dos scenario verification returned a status code that isn't currently supported: {0}", response.StatusCode));
            }

            Assert.Fail("Dos scenario failed: " + result.FailureReason);
            return result;
        }

        public async Task<VerificationResult> Verify(IEsbTestScenario scenario)
        {
            var request = new VerifyEsbTestScenarioPostRequest(scenario);
            var response = await _client.ExecutePostTaskAsync(request);
            VerificationResult result;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return JsonConvert.DeserializeObject<SuccessfulVerificationResult>(response.Content);
                case HttpStatusCode.NotFound:
                    result = JsonConvert.DeserializeObject<NoMatchingScenarioVerificationResult>(response.Content);
                    break;
                case HttpStatusCode.Conflict:
                    result = JsonConvert.DeserializeObject<IncorrectNumberOfRequestsVerificationResult<ITKDispatchRequest>>(response.Content);
                    break;
                case HttpStatusCode.BadRequest:
                    result = JsonConvert.DeserializeObject<RequestMismatchVerificationResult<ITKDispatchRequest>>(response.Content);
                    break;
                default:
                    throw new NotSupportedException(string.Format("ESB scenario verification returned a status code that isn't currently supported: {0}", response.StatusCode));
            }

            Assert.Fail("Esb scenario failed: " + result.FailureReason);
            return result;
        }

        private readonly IRestClient _client;
    }
}
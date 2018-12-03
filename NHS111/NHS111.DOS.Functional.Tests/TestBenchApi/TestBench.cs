
namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using RestSharp;
    using Newtonsoft.Json;

    public class TestBench {

        public TestBench() {
            _client = new RestClient("http://localhost:55954/");
        }

        public IDosTestScenarioSetup SetupDosScenario() {
            return new DosTestScenarioSetup();
        }

        public IEsbTestScenarioSetup SetupEsbScenario() {
            return new EsbTestScenarioSetup();
        }

        public async Task<DosVerificationResult> Verify(IDosTestScenario scenario) {
            var request = new VerifyDosTestScenarioPostRequest(scenario.Postcode);
            var response = await _client.ExecutePostTaskAsync(request);
            switch (response.StatusCode) {
                case HttpStatusCode.OK:
                    return new SuccessfulDosVerificationResult();
                case HttpStatusCode.NotFound: {
                    return JsonConvert.DeserializeObject<NoMatchingScenarioDosVerificationResult>(response.Content);
                }
                case HttpStatusCode.Conflict: {
                    return JsonConvert.DeserializeObject<IncorrectNumberOfRequestsDosVerificationResult>(response.Content);
                }
                case HttpStatusCode.BadRequest: {
                    return JsonConvert.DeserializeObject<RequestMismatchDosVerificationResult>(response.Content);
                }
            }

            throw new NotSupportedException(string.Format("Verify returned a status code that isn't currently supported: {0}", response.StatusCode));
        }

        public async Task Verify(IEsbTestScenario scenario) {
            var request = new VerifyEsbTestScenarioPostRequest(scenario);
            var response = await _client.ExecutePostTaskAsync(request);


        }

        private readonly IRestClient _client;
    }
}
namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using RestSharp;

    public class VerifyDosTestScenarioPostRequest
        : RestRequest
    {
        public VerifyDosTestScenarioPostRequest(ITestScenario scenario)
            : base("dostestscenario/verify", Method.POST, DataFormat.Json) {
            AddJsonBody(scenario.Postcode);
        }
    }
}
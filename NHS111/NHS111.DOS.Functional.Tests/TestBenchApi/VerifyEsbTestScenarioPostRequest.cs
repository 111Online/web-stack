namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using RestSharp;

    public class VerifyEsbTestScenarioPostRequest
        : RestRequest {
        public VerifyEsbTestScenarioPostRequest(IEsbTestScenario scenario)
            : base("esbtestscenario/verify", Method.POST, DataFormat.Json) {
            AddJsonBody(scenario.IncomingITKDispatchRequest.PatientDetails.CurrentAddress.PostalCode);
        }
    }
}
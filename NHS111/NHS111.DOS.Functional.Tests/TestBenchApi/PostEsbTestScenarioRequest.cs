namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using RestSharp;

    public class PostEsbTestScenarioRequest
        : RestRequest
    {
        public PostEsbTestScenarioRequest(IEsbTestScenario scenario)
            : base("esbtestscenario", Method.POST, DataFormat.Json)
        {
            AddJsonBody(scenario);
        }
    }
}
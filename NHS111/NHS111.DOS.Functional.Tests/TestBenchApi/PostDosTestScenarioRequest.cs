namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using RestSharp;

    public class PostDosTestScenarioRequest
        : RestRequest
    {
        public PostDosTestScenarioRequest(DosTestScenario scenario)
            : base("dostestscenario", Method.POST, DataFormat.Json)
        {
            AddJsonBody(scenario);
        }
    }
}
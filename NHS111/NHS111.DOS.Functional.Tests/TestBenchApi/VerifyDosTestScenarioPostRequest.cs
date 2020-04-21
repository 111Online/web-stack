namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using Models.Models.Business;
    using RestSharp;

    public class VerifyDosTestScenarioPostRequest
        : RestRequest
    {
        public VerifyDosTestScenarioPostRequest(Postcode postcode)
            : base("dostestscenario/verify", Method.POST, DataFormat.Json)
        {
            AddJsonBody(postcode.NormalisedValue);
        }
    }
}
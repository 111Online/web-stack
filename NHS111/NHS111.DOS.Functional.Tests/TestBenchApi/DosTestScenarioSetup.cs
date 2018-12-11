﻿namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using System.Threading.Tasks;
    using Models.Models.Web.DosRequests;
    using NUnit.Framework;
    using RestSharp;

    public interface IDosTestScenarioSetup {
        IDosRequestSetup ExpectingNoRequestsTo(IDosEndpoint endpoint);
        IDosRequestSetup ExpectingRequestTo(IDosEndpoint endpoint);
    }

    public interface IDosRequestSetup {
        IDosRequestSetup Matching(DosFilteredCase dosCase);
        IDosRequestSetup Returns(params DosTestScenarioTransformer[] transformers);
        IDosRequestSetup OtherwiseReturns(params DosTestScenarioTransformer[] mismatchTransformers);
        IDosTestScenarioSetup Then();
        Task<IDosTestScenario> BeginAsync();
    }

    public class DosTestScenarioSetup
        : IDosTestScenarioSetup, IDosRequestSetup {

        public DosTestScenarioSetup() {
            _client = new RestClient("http://localhost:55954/");
        }

        public IDosRequestSetup ExpectingNoRequestsTo(IDosEndpoint endpoint) {
            return this;
        }

        public IDosRequestSetup ExpectingRequestTo(IDosEndpoint endpoint) {
            _currentRequest = new DosTestScenarioRequest();
            return this;
        }

        public IDosRequestSetup Matching(DosFilteredCase dosCase) {
            _currentRequest.InboundDosFilteredCase = dosCase;
            return this;
        }

        public IDosRequestSetup Returns(params DosTestScenarioTransformer[] transformers) {
            _currentRequest.MatchSequence.AddRange(transformers);
            return this;
        }

        public IDosRequestSetup OtherwiseReturns(params DosTestScenarioTransformer[] mismatchTransformers) {
            _currentRequest.MismatchSequence.AddRange(mismatchTransformers);
            return this;
        }

        public IDosTestScenarioSetup Then() {
            FinaliseCurrentRequest();
            return this;
        }

        private void FinaliseCurrentRequest() {
            if (_currentRequest == null)
                return;
            _scenario.Requests.Add(_currentRequest);
            _currentRequest = null;
        }

        public async Task<IDosTestScenario> BeginAsync() {
            FinaliseCurrentRequest();
            var request = new PostDosTestScenarioRequest(_scenario);
            var response = await _client.ExecutePostTaskAsync<string>(request);
            Assert.True(response.IsSuccessful, response.ErrorMessage);
            _scenario.Postcode = response.Data;
            return _scenario;
        }

        private readonly DosTestScenario _scenario = new DosTestScenario();
        private readonly IRestClient _client;

        private DosTestScenarioRequest _currentRequest = null;
    }
}
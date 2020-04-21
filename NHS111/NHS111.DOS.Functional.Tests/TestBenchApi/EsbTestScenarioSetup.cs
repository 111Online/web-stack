namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using Models.Models.Web.ITK;
    using RestSharp;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public interface IEsbTestScenarioSetup
    {
        IEsbRequestSetup ExpectingRequestTo(IEsbEndpoint endpoint);
        IEsbRequestSetup ExpectingNoRequestTo(IEsbEndpoint endpoint);
    }

    public interface IEsbRequestSetup
    {
        IEsbRequestSetup Matching(ITKDispatchRequest itkDispatchRequest);
        IEsbRequestSetup Returns(HttpStatusCode statusCode);
        IEsbRequestSetup OtherwiseReturns(HttpStatusCode statusCode);
        Task<IEsbTestScenario> BeginAsync();
    }

    public class EsbTestScenarioSetup
        : IEsbTestScenarioSetup, IEsbRequestSetup
    {

        public EsbTestScenarioSetup()
        {
            _client = new RestClient("http://localhost:55954/");
        }

        public IEsbRequestSetup ExpectingRequestTo(IEsbEndpoint endpoint)
        {
            _scenario = new EsbTestScenario
            {
                Requests = new List<EsbTestScenarioRequest> { new EsbTestScenarioRequest { Expected = true } }
            };
            return this;
        }

        public IEsbRequestSetup ExpectingNoRequestTo(IEsbEndpoint endpoint)
        {
            _scenario = new EsbTestScenario
            {
                Requests = new List<EsbTestScenarioRequest> { new EsbTestScenarioRequest { Expected = false } }
            };
            return this;
        }

        public IEsbRequestSetup Matching(ITKDispatchRequest itkDispatchRequest)
        {
            _scenario.IncomingITKDispatchRequest = itkDispatchRequest;
            _scenario.Requests.First().InboundITKDispatchRequest = itkDispatchRequest;
            return this;
        }

        public IEsbRequestSetup Returns(HttpStatusCode statusCode)
        {
            _scenario.MatchStatusCode = (int)statusCode;
            return this;
        }

        public IEsbRequestSetup OtherwiseReturns(HttpStatusCode statusCode)
        {
            _scenario.MismatchStatusCode = (int)statusCode;
            return this;
        }

        public async Task<IEsbTestScenario> BeginAsync()
        {
            var request = new PostEsbTestScenarioRequest(_scenario);
            await _client.ExecutePostTaskAsync(request);
            return _scenario;
        }

        private IEsbTestScenario _scenario;
        private readonly IRestClient _client;
    }
}
using log4net;
using RestSharp;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NHS111.Utils.RestTools
{
    public interface ILoggingRestClient
    {
        Task<IRestResponse> ExecuteAsync(IRestRequest request);
        Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request);
    }

    public class LoggingRestClient : ILoggingRestClient
    {
        private readonly ILog _logger;
        private readonly IRestClient _restClient;

        public LoggingRestClient(string baseUrl, ILog logger, int defaultConnectionLimit = 5)
        {
            ServicePointManager.DefaultConnectionLimit = defaultConnectionLimit;
            _logger = logger;
            
            _restClient = new RestClient(baseUrl);
            InitializeSerializationHandlers(_restClient);
        }

        private void InitializeSerializationHandlers(IRestClient client)
        {
            client.AddHandler("application/json", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("text/json", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("text/x-json", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("text/javascript", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("*+json", () => NewtonsoftJsonSerializer.Default);
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest request)
        {
            _logger.Info(string.Format("Request to: {0} performed", _restClient.BuildUri(request)));

            IRestResponse response = await _restClient.ExecuteAsync(request, request.Method, CancellationToken.None);

            if (response == null)
            {
                response = new RestResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "API response was null"
                };
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error(string.Format("Request to: {0} returned with Error Code: {1} and response: {2}", _restClient.BuildUri(request), response.StatusCode, response.ErrorMessage));
            }

            return response;
        }
        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request)
        {
            _logger.Info(string.Format("Request to: {0} performed", _restClient.BuildUri(request)));

            var response = await _restClient.ExecuteAsync<T>(request);

            if (response == null)
            {
                response = new RestResponse<T>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "API response was null"
                };
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error(string.Format("Request to: {0} returned with Error Code: {1} and response: {2}", _restClient.BuildUri(request), response.StatusCode, response.ErrorMessage));
            }

            return response;
        }
    }
}

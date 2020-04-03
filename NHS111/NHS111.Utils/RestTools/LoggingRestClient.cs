using System;
using System.Net;
using System.Threading.Tasks;
using log4net;
using RestSharp;

namespace NHS111.Utils.RestTools
{
    public class LoggingRestClient : RestClient
    {
        private readonly ILog _logger;

        [Obsolete("Pass DefaultConnectionLimit from IConfiguration.ServicePointManagerDefaultConnectionLimit")]
        public LoggingRestClient(string baseUrl, ILog logger) : this (baseUrl, logger, 5) {}
        
        public LoggingRestClient(string baseUrl, ILog logger, int defaultConnectionLimit) : base(baseUrl)
        {
            ServicePointManager.DefaultConnectionLimit = defaultConnectionLimit;
            _logger = logger;
            InitializeSerialisationHandlers();
        }

        private void InitializeSerialisationHandlers()
        {
            this.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
            this.AddHandler("text/json", NewtonsoftJsonSerializer.Default);
            this.AddHandler("text/x-json", NewtonsoftJsonSerializer.Default);
            this.AddHandler("text/javascript", NewtonsoftJsonSerializer.Default);
            this.AddHandler("*+json", NewtonsoftJsonSerializer.Default);
        }

        public override async Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            _logger.Info(string.Format("Request to: {0}{1} performed", BaseUrl, request.Resource));

            var response = await base.ExecuteTaskAsync(request);

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
                _logger.Error(string.Format("Request to: {0}{1} returned with Error Code: {2} and response: {3}", BaseUrl, request.Resource, response.StatusCode, response.ErrorMessage));
            }

            return response;
        }
        public override async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            _logger.Info(string.Format("Request to: {0}{1} performed", BaseUrl, request.Resource));

            var response = await base.ExecuteTaskAsync<T>(request);

            if (response == null)
            {
                response = new RestResponse<T>
                {
                    StatusCode = HttpStatusCode.BadRequest, ErrorMessage = "API response was null"
                };
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error(string.Format("Request to: {0}{1} returned with Error Code: {2} and response: {3}", BaseUrl, request.Resource, response.StatusCode, response.ErrorMessage));
            }

            return response;
        }
    }
}

using log4net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NHS111.Utils.RestTools
{
    public interface ILoggingRestClient
    {
        Task<IRestResponse> ExecuteAsync(IRestRequest request);
        Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request);
    }

    public class LoggingRestClient : ILoggingRestClient
    {
        private const int RetryDelayMs = 1500;

        private readonly ILog _logger;
        private readonly IRestClient _restClient;

        private readonly TelemetryClient _tc = new TelemetryClient();

        public LoggingRestClient(string baseUrl, ILog logger, int defaultConnectionLimit = 5, int timeout = 30000)
        {
            ServicePointManager.DefaultConnectionLimit = defaultConnectionLimit;
            _logger = logger;

            _restClient = new RestClient(baseUrl);
            _restClient.Timeout = timeout;
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

            var startTime = DateTimeOffset.UtcNow;
            var sw = Stopwatch.StartNew();
            IRestResponse response = await _restClient.ExecuteAsync(request, request.Method, CancellationToken.None);
            sw.Stop();

            // Retry once in case of unsuccessful calls
            if (ShouldBeRetried(response))
            {
                TrackExceptionToAppInsights(request, response, sw.Elapsed, true);
                await Task.Delay(RetryDelayMs);
                startTime = DateTimeOffset.UtcNow;
                sw.Restart();
                response = await _restClient.ExecuteAsync(request, request.Method, CancellationToken.None);
                sw.Stop();
            }

            if (response == null)
            {
                response = new RestResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "API response was null"
                };
            }

            if (!response.IsSuccessful)
            {
                TrackExceptionToAppInsights(request, response, sw.Elapsed);
                _logger.Error(string.Format("Request to: {0} returned with Error Code: {1} and response: {2}", _restClient.BuildUri(request), response.StatusCode, response.ErrorMessage));
            }

            return response;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request)
        {
            _logger.Info(string.Format("Request to: {0} performed", _restClient.BuildUri(request)));

            var startTime = DateTimeOffset.UtcNow;
            var sw = Stopwatch.StartNew();
            var response = await _restClient.ExecuteAsync<T>(request);
            sw.Stop();

            // Retry once in case of unsuccessful calls
            if (ShouldBeRetried(response))
            {
                TrackExceptionToAppInsights(request, response, sw.Elapsed, true);
                await Task.Delay(RetryDelayMs);
                startTime = DateTimeOffset.UtcNow;
                sw.Restart();
                response = await _restClient.ExecuteAsync<T>(request);
                sw.Stop();
            }

            if (response == null)
            {
                response = new RestResponse<T>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "API response was null"
                };
            }

            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.NotFound) // Do not track exception for NotFound. This is an expected result in some cases
            {
                TrackExceptionToAppInsights(request, response, sw.Elapsed);
                _logger.Error(string.Format("Request to: {0} returned with Error Code: {1} and response: {2}", _restClient.BuildUri(request), response.StatusCode, response.ErrorMessage));
            }

            return response;
        }


        /// <summary>
        /// Determine whether a request should be retried or not
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool ShouldBeRetried(IRestResponse response)
        {
            if (response == null)
            {
                return true;
            }
            else if (response.IsSuccessful) // any 2xx response codes are of course not to be retried
            {
                return false;
            }
            else if(response.StatusCode == 0 || response.ErrorException != null)
            {
                return true;
            }
            else
            {
                switch (response.StatusCode)
                {
                    // For those status codes we do want to retry. Everything else, like 404 not.
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.RequestTimeout:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.GatewayTimeout:
                    case HttpStatusCode.ServiceUnavailable:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Tracks an unsuccessful request to Application Insights
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void TrackExceptionToAppInsights(IRestRequest request, IRestResponse response, TimeSpan duration, bool willRetry = false)
        {
            string message = "Unsuccessful HTTP call - " + (willRetry ? $"will be retried after {RetryDelayMs}ms" : "will not be retried");

            var ex = new HttpException(message, response?.ErrorException);
            var telemetry = new ExceptionTelemetry(ex);
            telemetry.Message = message;
            telemetry.Metrics.Add("Duration", duration.TotalMilliseconds);

            telemetry.Properties.Add("Endpoint", _restClient.BaseUrl.ToString());
            telemetry.Properties.Add("Resource", request.Resource);
            telemetry.Properties.Add("Method", request.Method.ToString());
            telemetry.Properties.Add("Attempt", request.Attempts.ToString());
            if (response != null)
            {
                telemetry.Properties.Add("HttpStatusCode", response.StatusCode.ToString());
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    telemetry.Properties.Add("ErrorMessage", response.ErrorMessage);
                }
            }
            _tc.TrackException(telemetry);
        }
    }
}

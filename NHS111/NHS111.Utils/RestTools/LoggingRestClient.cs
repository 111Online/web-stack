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
        private string _baseUrlDefault;
        public LoggingRestClient(string baseUrl, ILog logger) : base(baseUrl)
        {
            ServicePointManager.DefaultConnectionLimit = 5;
            _logger = logger;
            InitializeSerialisationHandlers();
            _baseUrlDefault = baseUrl;
        }

        private void InitializeSerialisationHandlers()
        {
            this.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
            this.AddHandler("text/json", NewtonsoftJsonSerializer.Default);
            this.AddHandler("text/x-json", NewtonsoftJsonSerializer.Default);
            this.AddHandler("text/javascript", NewtonsoftJsonSerializer.Default);
            this.AddHandler("*+json", NewtonsoftJsonSerializer.Default);
        }

        public override async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            base.BaseUrl = new Uri(_baseUrlDefault);
            if (RequestContainsAuthority(request))
            {
                base.BaseUrl = new Uri(new Uri(request.Resource.TrimStart('/')).GetLeftPart(UriPartial.Authority));
                request.Resource = new Uri(request.Resource.TrimStart('/')).PathAndQuery;
            }
            
            _logger.Info(string.Format("Request to: {0}{1} performed", BaseUrl,request.Resource));

            var response = await base.ExecuteTaskAsync<T>(request);
            if(response != null && response.StatusCode !=  HttpStatusCode.OK) _logger.Error(String.Format("Request to: {0}{1} returned with Error Code: {2} and response: {3}", BaseUrl, request.Resource, response.StatusCode, response.ErrorMessage));
            else _logger.Error(String.Format("Request to: {0}{1} failed", BaseUrl, request.Resource));

            base.BaseUrl = new Uri(_baseUrlDefault); //set base url back to default setting
            return response;
        }

        private bool RequestContainsAuthority(IRestRequest request)
        {
            return Uri.IsWellFormedUriString(request.Resource.TrimStart('/'), UriKind.Absolute);
        }
    }
}

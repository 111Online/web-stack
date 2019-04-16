﻿using System;
using System.Net;
using System.Threading.Tasks;
using log4net;
using RestSharp;

namespace NHS111.Utils.RestTools
{
    public class LoggingRestClient : RestClient
    {
        private readonly ILog _logger;
        public LoggingRestClient(string baseUrl, ILog logger) : base(baseUrl)
        {
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

        public override async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            _logger.Info(string.Format("Request to: {0}{1} performed", BaseUrl,request.Resource));
            var response = await base.ExecuteTaskAsync<T>(request);
            if(response.StatusCode !=  HttpStatusCode.OK) _logger.Error(String.Format("Request to: {0} returned with Error Code: {1} and response: {2}", response.ResponseUri.AbsoluteUri, response.StatusCode, response.ErrorMessage));
            return response;
        }
    }
}

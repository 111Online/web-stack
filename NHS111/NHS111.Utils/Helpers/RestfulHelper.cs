using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Helpers
{
    using System.IO;
    using System.Web;

    public class RestfulHelper : IRestfulHelper
    {
        private readonly WebClient _webClient;
        private readonly HttpClient _httpClient;

        public RestfulHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public RestfulHelper()
        {
            _webClient = new WebClient();
            _httpClient = new HttpClient();

        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                return await _webClient.DownloadStringTaskAsync(new Uri(url));
            }
            catch (WebException e) {
                using (var stream = new StreamReader(e.Response.GetResponseStream())) {
                    throw new WebException(
                        string.Format("There was a problem requesting '{0}'; {1}", url, stream.ReadToEnd()), e);
                }
            }
        }

        public async Task<string> GetAsync(string url, ICredentials credentials)
        {
            try
            {
                _webClient.Credentials = credentials;
                return await _webClient.DownloadStringTaskAsync(new Uri(url));
            }
            catch (WebException e)
            {
                using (var stream = new StreamReader(e.Response.GetResponseStream()))
                {
                    throw new WebException(
                        string.Format("There was a problem requesting '{0}'; {1}", url, stream.ReadToEnd()), e);
                }
            }
        }


        public async Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request)
        {
            var httpRequestMessage = await BuildRequestMessage(url, request);
            return await _httpClient.SendAsync(httpRequestMessage);
       
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request, Dictionary<string, string> headers)
        {
            var httpRequestMessage = await BuildRequestMessage(url, request);
            foreach (var header in headers)
            {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
            return await _httpClient.SendAsync(httpRequestMessage);
        }

        private async Task<HttpRequestMessage> BuildRequestMessage(string url, HttpRequestMessage request)
        {
            var data = await request.Content.ReadAsStringAsync();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Content = new StringContent(data, Encoding.UTF8, "application/json"),
                Version = HttpVersion.Version10 //forcing 1.0 to prevent Expect 100 Continue header
            };
            return httpRequestMessage;
        }

    
    }

    public interface IRestfulHelper
    {
        Task<string> GetAsync(string url);
        Task<string> GetAsync(string url, ICredentials credentials);
        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request);
        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request, Dictionary<string, string> headers);
    }
}
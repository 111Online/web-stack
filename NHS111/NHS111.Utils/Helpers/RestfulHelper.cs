using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Helpers
{
    public class RestfulHelper : IRestfulHelper
    {
        private readonly WebClient _webClient;

        public RestfulHelper()
        {
            _webClient = new WebClient();
        }

        public async Task<string> GetAsync(string url)
        {
            return await _webClient.DownloadStringTaskAsync(new Uri(url));
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request)
        {
            var data = await request.Content.ReadAsStringAsync();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Content = new StringContent(data, Encoding.UTF8, "application/json"),
                Version = HttpVersion.Version10 //forcing 1.0 to prevent Expect 100 Continue header
        };
            foreach (var header in request.Headers) {
                Console.WriteLine(header.Key + ": " + string.Join(", ", header.Value));
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
            return await new HttpClient().SendAsync(httpRequestMessage);
        }
    }

    public interface IRestfulHelper
    {
        Task<string> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request);
    }
}
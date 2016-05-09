namespace NHS111.Business.Feedback.Api.Functional.Tests {
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class RestfulHelper : IRestfulHelper
    {
        private readonly WebClient _webClient;

        public RestfulHelper()
        {
            _webClient = new WebClient();
        }

        public async Task<string> GetAsync(string url)
        {
            _webClient.Credentials = new NetworkCredential("nhsUser", "oD4rqw4Ntr");
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
            foreach (var header in request.Headers)
            {
                Console.WriteLine(header.Key + ": " + string.Join(", ", header.Value));
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
            return await new HttpClient().SendAsync(httpRequestMessage);
        }
    }
}
namespace NHS111.Utils.Helpers
{
    using System.Net;
    using System.Net.Http;

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Get(string username, string password)
        {
            var networkCredential = new NetworkCredential(username, password);
            var httpClientHandler = new HttpClientHandler { Credentials = networkCredential };
            return new HttpClient(httpClientHandler);
        }
    }

    public interface IHttpClientFactory
    {
        HttpClient Get(string username, string password);
    }

}
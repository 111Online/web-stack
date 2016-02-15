namespace NHS111.Business.API.Functional.Tests {
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IRestfulHelper
    {
        Task<string> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request);
    }
}
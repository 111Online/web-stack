using System.Net.Http;
using System.Text;

namespace NHS111.Functional.Tests.Tools
{
    public class RequestFormatting
    {
        public static HttpRequestMessage CreateHTTPRequest(string requestContent, string requestContentWrapper = "\"")
        {
            var debugging = new HttpRequestMessage
            {
                Content = new StringContent(requestContentWrapper + requestContent + requestContentWrapper, Encoding.UTF8, "application/json")
            };

            return debugging;
        }
    }
}

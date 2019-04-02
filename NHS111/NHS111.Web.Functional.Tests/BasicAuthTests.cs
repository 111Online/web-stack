
namespace NHS111.Web.Functional.Tests {
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class BasicAuthTests {
        [SetUp]
        public void SetUp() {
            var testUrl = ConfigurationManager.AppSettings["UnauthenticatedTestWebsiteUrl"];
            if (string.IsNullOrEmpty(testUrl))
                throw new ConfigurationErrorsException("This test requires an address without credentials in the config called 'UnauthenticatedTestWebsiteUrl'");
            _client = new HttpClient {BaseAddress = new Uri(testUrl)};
        }

        [Test]
        public async Task Request_Always_RequiresBasicAuth() {
            var response = await _client.GetAsync("/PW755/Male/22/start");
            Assert.True(response.RequiresBasicAuth());
        }

        [Test]
        public async Task Request_OutsideOfRoot_OnlyAuthenticatesOnce() {
            var nonRootResource = "/PW755/Male/22/start?args=E34F21A2ECD98C0DCCEBE9FF42B39CB400623EA70B147980D5B2E98FAA5BF7D36CC86FDAD1B49197D6FE9C58FBD0250BC1C380D047F72AB401D970AEEAFA8CBF8EB575B2E844F2081AE0F32F49C1D2EE5E39F70714A319F154374A65F579195F0D5804C32AAB385A952E4EB6660C10058A6111635C10B87644D843D5B7909FABE560ED58D77D366F12D747AB8DE5B7B02A20BB380A6C9DEE569B1A5A269EF21E13C9D250E57462DF5DCF81A9C62AE878861DE946ABBB4CE96EA416B443A256477DE33194087DDAB273FF7B119D8B1AE3138BD7FF5CE857085F5C3179B29D9B71";
            var firstResponse = await _client.GetAsync(nonRootResource);
            Assert.True(firstResponse.RequiresBasicAuth());
            
            var request = new HttpRequestMessage(HttpMethod.Get, nonRootResource);
            request.Headers.Authorization = EncodeCredentials();
            var secondResponse = await _client.SendAsync(request);
            Assert.False(secondResponse.RequiresBasicAuth());
        }

        private AuthenticationHeaderValue EncodeCredentials() {
            var username = ConfigurationManager.AppSettings["login_credential_user"];
            var password = ConfigurationManager.AppSettings["login_credential_password"];
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ConfigurationErrorsException("This test requires the basic auth username and password in the config called 'login_credential_user' and 'login_credential_password' respectively");

            var bytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + password);
            var encodedCredentials = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", encodedCredentials);
        }

        private HttpClient _client;
    }

    public static class HttpResponseMessageExtensions {
        public static bool RequiresBasicAuth(this HttpResponseMessage response) {
            return response.StatusCode == HttpStatusCode.Unauthorized &&
                   response.Headers.WwwAuthenticate.Any();
        }
    }
}
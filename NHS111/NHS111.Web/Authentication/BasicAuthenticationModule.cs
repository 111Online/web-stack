

namespace NHS111.Web.Authentication {
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    public class BasicAuthenticationAttribute
        : ActionFilterAttribute {

        public string Realm { get; set; }

        public BasicAuthenticationAttribute(string username, string password, string realm = "NHS111") {
            Realm = realm;
            _username = username;
            _password = password;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            // Allow requests even when not authenticated to actions with the AllowAnonymousAccess attribute present. see below.
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAccess), false).Any())
            {
                return;
            }

            var credentials = filterContext.HttpContext.Request.DecodeBasicAuthCredentials();

            if (!credentials.Validate(_username, _password))
                filterContext.HttpContext.Response.IssueAuthChallenge(Realm);
        }

        private readonly string _username;
        private readonly string _password;
    }

    internal static class NetworkCredentialsExtensions {
        public static bool Validate(this NetworkCredential credentials, string username, string password) {
            return credentials.UserName == username &&
                   credentials.Password == password;
        }
    }

    internal static class HttpResponseBaseExtensions {
        public static void IssueAuthChallenge(this HttpResponseBase response, string realm) {
            if (response.HeadersWritten)
                return;
            response.StatusCode = 401;
            response.AddHeader("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", realm));
            response.End();
        }
    }

    internal static class HttpRequestBaseExtensions {
        public static NetworkCredential DecodeBasicAuthCredentials(this HttpRequestBase request) {
            var authHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader))
                return new NetworkCredential();

            try {
                var encodedCredentials = authHeader.Replace("Basic ", "");
                var bytes = Convert.FromBase64String(encodedCredentials);
                var decodedAuthHeader = Encoding.ASCII.GetString(bytes);
                var credentials = decodedAuthHeader.Split(':');

                return new NetworkCredential(credentials[0], credentials[1]);

            } catch (Exception e) {
                throw new FormatException("The provided basic auth header was not in the expected format", e);
            }
        }
    }

    /// <summary>
    // Attribute to mark a controller as accessible regarding of authentication requirement
    // Source: https://stackoverflow.com/a/9963113/1537195
    /// </summary>
    public class AllowAnonymousAccess : Attribute
    {
    }
}
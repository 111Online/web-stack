using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NHS111.Business.Feedback.Api.Authentication
{
    public class BasicAuthenticationModule : IHttpModule
    {
        /// <summary>
        /// HTTP1.1 Authorization header
        /// </summary> 
        public const string HttpAuthorizationHeader = "Authorization";

        /// <summary>
        /// HTTP1.1 Basic Challenge Scheme Name
        /// </summary>
        public const string HttpBasicSchemeName = "Basic"; // 

        /// <summary>
        /// HTTP1.1 Credential username and password separator
        /// </summary>
        public const char HttpCredentialSeparator = ':';

        /// <summary>
        /// HTTP1.1 Not authorized response status code
        /// </summary>
        public const int HttpNotAuthorizedStatusCode = 401;

        /// <summary>
        /// HTTP1.1 Basic Challenge Scheme Name
        /// </summary>
        public const string HttpWwwAuthenticateHeader = "WWW-Authenticate";

        /// <summary>
        /// The name of cookie that is sent to client
        /// </summary>
        public const string AuthenticationCookieName = "BasicAuthentication";

        /// <summary>
        /// HTTP.1.1 Basic Challenge Realm
        /// </summary>
        public const string Realm = "demo";

        /// <summary>
        /// Dictionary that caches whether basic authentication challenge should be sent. Key is request URL + request method, value indicates whether
        /// challenge should be sent.
        /// </summary>
        private static readonly IDictionary<string, bool> ShouldChallengeCache = new Dictionary<string, bool>();

        protected bool ValidateCredentials(string userName, string password)
        {
            var configUsername = ConfigurationManager.AppSettings["login_credential_user"];
            var configPassword = ConfigurationManager.AppSettings["login_credential_password"];

            return configUsername != null && configUsername == userName && configPassword != null  && configPassword == password;
        }

        protected bool ExtractBasicCredentials(string authorizationHeader, ref string username, ref string password)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return false;
            }

            string verifiedAuthorizationHeader = authorizationHeader.Trim();
            if (verifiedAuthorizationHeader.IndexOf(HttpBasicSchemeName, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return false;
            }

            // get the credential payload
            verifiedAuthorizationHeader = verifiedAuthorizationHeader.Substring(HttpBasicSchemeName.Length, verifiedAuthorizationHeader.Length - HttpBasicSchemeName.Length).Trim();
            // decode the base 64 encoded credential payload
            byte[] credentialBase64DecodedArray = Convert.FromBase64String(verifiedAuthorizationHeader);
            string decodedAuthorizationHeader = Encoding.UTF8.GetString(credentialBase64DecodedArray, 0, credentialBase64DecodedArray.Length);

            // get the username, password, and realm
            int separatorPosition = decodedAuthorizationHeader.IndexOf(HttpCredentialSeparator);

            if (separatorPosition <= 0)
            {
                return false;
            }

            username = decodedAuthorizationHeader.Substring(0, separatorPosition).Trim();
            password = decodedAuthorizationHeader.Substring(separatorPosition + 1, (decodedAuthorizationHeader.Length - separatorPosition - 1)).Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            return true;
        }

        public void AuthenticateUser(Object source, EventArgs e)
        {
            var context = ((HttpApplication)source).Context;

            string authorizationHeader = context.Request.Headers[HttpAuthorizationHeader];

            // Extract the basic authentication credentials from the request
            string userName = null;
            string password = null;
            if (!this.ExtractBasicCredentials(authorizationHeader, ref userName, ref password) || !this.ValidateCredentials(userName, password))
            {
                IssueAuthenticationChallenge(source, e);
                return;
            }

            // check whether cookie is set and send it to client if needed
            var authCookie = context.Request.Cookies.Get(AuthenticationCookieName);
            if (authCookie == null)
            {
                authCookie = new HttpCookie(AuthenticationCookieName, "1") { Expires = DateTime.Now.AddHours(1) };
                context.Response.Cookies.Add(authCookie);
            }
        }

        public void IssueAuthenticationChallenge(Object source, EventArgs e)
        {
            var context = ((HttpApplication)source).Context;

            if (ShouldChallenge(context))
            {
                // if authentication cookie is not set issue a basic challenge
                var authCookie = context.Request.Cookies.Get(AuthenticationCookieName);
                if (authCookie == null)
                {
                    //make sure that user is not authencated yet
                    if (!context.Response.Cookies.AllKeys.Contains(AuthenticationCookieName))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = HttpNotAuthorizedStatusCode;
                        context.Response.AddHeader(HttpWwwAuthenticateHeader, "Basic realm =\"" + Realm + "\"");
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if authentication challenge should be sent to client based on configured exclude rules
        /// </summary>
        private bool ShouldChallenge(HttpContext context)
        {
            // first check cache
            var key = string.Concat(context.Request.Path, context.Request.HttpMethod);
            if (ShouldChallengeCache.ContainsKey(key))
            {
                return ShouldChallengeCache[key];
            }

            ShouldChallengeCache[key] = true;
            return true;
        }

        private static bool IsRedirect(int httpStatusCode)
        {
            return new[]
            {
                HttpStatusCode.MovedPermanently,
                HttpStatusCode.Redirect,
                HttpStatusCode.TemporaryRedirect
            }.Any(c => (int)c == httpStatusCode);
        }

        public void Init(HttpApplication context)
        {
            // Subscribe to the authenticate event to perform the authentication.
            context.AuthenticateRequest += AuthenticateUser;

            // Subscribe to the EndRequest event to issue the authentication challenge if necessary.
            context.EndRequest += IssueAuthenticationChallenge;
        }

        public void Dispose()
        {
            // do nothing here
        }
    }
}
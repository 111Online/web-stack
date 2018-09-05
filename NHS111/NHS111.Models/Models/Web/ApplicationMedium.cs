using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web
{
    public static class ApplicationMediums
    {
        private const String _referralCookieName = "referrer";

        private static readonly List<ApplicationMedium> _applicationMediums = new List<ApplicationMedium>()
        {
            new ApplicationMedium("nhsapp", "NHS App"),
            new ApplicationMedium("webdirect", "direct")
        };

        public static ApplicationMedium NhsApp = _applicationMediums.Find(m => m.ValueEquals("nhsapp"));
        public static ApplicationMedium WebDirect = _applicationMediums.Find(m => m.ValueEquals("webdirect"));

        public static ApplicationMedium GetFromRequest(HttpRequestBase request)
        {
            if (request.Cookies[_referralCookieName] != null)
                return _applicationMediums.Find(m => m.ToString() == request.Cookies[_referralCookieName].Value);
            else
                return WebDirect;
        }

        public static void SetFromRequest(ResultExecutingContext filterContext)
        {
            foreach (ApplicationMedium medium in _applicationMediums)
            {
                var httpContext = filterContext.RequestContext.HttpContext;

                if (httpContext.Request.QueryString["utm_medium"] != null && medium.QueryStringEquals(httpContext.Request.QueryString["utm_medium"]))
                {
                    if (httpContext.Response.Cookies[_referralCookieName] != null)
                    {
                        httpContext.Response.Cookies[_referralCookieName].Value = medium.ToString();
                        httpContext.Request.Cookies[_referralCookieName].Value = medium.ToString();
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie(_referralCookieName)
                        {
                            Value = medium.ToString(),
                            Expires = DateTime.MinValue
                        };
                        httpContext.Response.Cookies.Add(cookie);
                        httpContext.Request.Cookies.Add(cookie);
                    }
                }
            }
        }
    }

    public class ApplicationMedium
    {
        public ApplicationMedium(string value, string querystringValue = null)
        {
            _value = value;

            _querystringValue = querystringValue;
        }

        public bool ValueEquals(string value)
        {
            return _value.ToLower().Equals(value.ToLower());
        }

        public bool QueryStringEquals(string queryStringValue)
        {
            return _querystringValue.ToLower().Equals(queryStringValue.ToLower());
        }

        private readonly string _value;
        private readonly string _querystringValue;
    }
}

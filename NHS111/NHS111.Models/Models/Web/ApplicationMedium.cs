using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web
{

    public static class ApplicationMediums
    {
        private static List<ApplicationMedium> _applicationMediums = new List<ApplicationMedium>()
        {
            new ApplicationMedium("nhsapp", "NHS App"),
            new ApplicationMedium("webdirect", "direct")
        };

        public static ApplicationMedium NhsApp = _applicationMediums.Find(m => m.ToString() == "nhsapp");
        public static ApplicationMedium WebDirect = _applicationMediums.Find(m => m.ToString() == "webdirect");

        public static ApplicationMedium GetFromRequest(HttpRequestBase request)
        {
            if (request.Cookies["referrer"] != null)
                return _applicationMediums.Find(m => m.ToString() == request.Cookies["referrer"].Value);
            else return WebDirect;
        }

        public static void SetFromRequest(ResultExecutingContext filterContext)
        {
            foreach (ApplicationMedium medium in _applicationMediums)
            {
                if (filterContext.RequestContext.HttpContext.Request.QueryString["utm_medium"] != null && filterContext.RequestContext.HttpContext.Request.QueryString["utm_medium"].ToLower() == medium._querystringValue.ToLower())
                {

                    if (filterContext.RequestContext.HttpContext.Response.Cookies["referrer"] != null)
                    {
                        filterContext.RequestContext.HttpContext.Request.Cookies["referrer"].Value =
                            medium.ToString();
                        filterContext.RequestContext.HttpContext.Response.Cookies["referrer"].Value =
                            medium.ToString();
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie("referrer");
                        cookie.Value = medium.ToString();
                        cookie.Expires = DateTime.MinValue;
                        filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie);
                        filterContext.RequestContext.HttpContext.Request.Cookies.Add(cookie);
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

        public override string ToString()
        {
            return _value;
        }

        private readonly string _value;
        public readonly string _querystringValue;
    }
}


using System.Net;
using System.Web.Http;

namespace NHS111.Business.DOS.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Enable TLS 1.2 for outbound connections so that TLS 1.2 on the downstream App Services can be enabled
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

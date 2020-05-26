using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace NHS111.Features
{
    public static class FeatureRouter
    {
        public static bool CovidSearchRedirect(NameValueCollection queryString)
        {
            bool queryParameterFlag, environmentVariable;
            //bool.TryParse(queryString["on2165"], out queryParameterFlag);
            bool.TryParse(ConfigurationManager.AppSettings["CovidSearchRedirectFlag"], out environmentVariable);
            return environmentVariable;
        }
    }
}
using System.Web;
using System.Web.Mvc;

namespace NHS111.Integration.MobileDOS.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

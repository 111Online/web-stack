using NHS111.Web.App_Start;
using System.Web.Mvc;

namespace NHS111.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
            filters.Add(new ApplicationMediumActionFilter());
        }
    }
}


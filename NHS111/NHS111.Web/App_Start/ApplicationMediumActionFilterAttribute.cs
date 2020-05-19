using NHS111.Models.Models.Web;
using System.Web.Mvc;

namespace NHS111.Web.App_Start
{

    public class ApplicationMediumActionFilterAttribute : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ApplicationMediums.SetFromRequest(filterContext);
            filterContext.Controller.ViewBag.Medium = ApplicationMediums.GetFromRequest(filterContext.RequestContext.HttpContext.Request);

        }
    }

}
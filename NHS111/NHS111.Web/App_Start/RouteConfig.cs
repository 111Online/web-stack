using System.Web.Mvc;
using System.Web.Routing;

namespace NHS111.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Gender",
                url: "Gender/{pathwayTitle}",
                defaults: new { controller = "Question", action = "GenderDirect" }
                );

            routes.MapRoute(
                name: "Slider",
                url: "Slider/{pathwayTitle}/{gender}/{age}",
                defaults: new { controller = "Question", action = "SliderDirect" }
                );

            routes.MapRoute(
                name: "Question",
                url: "Question/JustToBeSafe/{pathwayId}/{age}/{pathwayTitle}",
                defaults: new { controller = "Question", action = "QuestionDirect", age = UrlParameter.Optional, pathwayTitle = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Location", action = "Home", id = UrlParameter.Optional }
                );
        }
    }
}

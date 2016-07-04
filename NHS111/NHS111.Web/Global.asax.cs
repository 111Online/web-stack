
namespace NHS111.Web {
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Models.Models.Web;
    using Presentation.ModelBinders;

    public class MvcApplication
        : System.Web.HttpApplication {

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders[typeof (JourneyViewModel)] = new JourneyViewModelBinder();
        }
    }
}
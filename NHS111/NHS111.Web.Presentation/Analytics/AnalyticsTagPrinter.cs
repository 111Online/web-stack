
namespace NHS111.Web.Presentation.Analytics {
    using System.Web;

    public abstract class AnalyticsTagPrinter {

        public abstract HtmlString Print();

        public abstract HtmlString PrintNoScript();
    }
}

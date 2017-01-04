
namespace NHS111.Web.Presentation.Analytics {
    using System.Collections.Generic;
    using System.Web;

    public abstract class AnalyticsTagPrinter {

        protected AnalyticsTagPrinter(Dictionary<string, string> data) {
            _data = data;
        }

        public abstract HtmlString Print();

        public abstract HtmlString PrintNoScript();

        protected readonly Dictionary<string, string> _data;
    }
}

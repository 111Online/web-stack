
using NHS111.Models.Models.Web;
using System.Linq;

namespace NHS111.Web.Presentation.Analytics
{
    using System.Web;

    public class AnalyticsTagPrinter
    {
        public AnalyticsTagPrinter()
        {
            DataLayerVariableName = "dataLayer";
        }

        public HtmlString Print(AnalyticsDataLayerContainer dataLayer)
        {
            var values = string.Join(",\n", dataLayer.Select(i => string.Format("'{0}': '{1}'", i.Key, i.Value)));
            var dataLayerScript = string.Format("{0} = [{{\n{1}\n}}];", DataLayerVariableName, values);
            return new HtmlString(dataLayerScript);
        }

        public string DataLayerVariableName { get; set; }
    }
}

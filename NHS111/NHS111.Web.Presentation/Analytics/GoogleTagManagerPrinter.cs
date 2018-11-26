namespace NHS111.Web.Presentation.Analytics {
    using System.Linq;
    using System.Web;
    using NHS111.Models.Models.Web;

    public class GoogleTagManagerPrinter
        : AnalyticsTagPrinter {

        public GoogleTagManagerPrinter(string containerId) {
            _containerId = containerId;

            DataLayerVariableName = "dataLayer";
        }

        public HtmlString Print(GoogleAnalyticsDataLayerContainer dataLayer) {
            var values = string.Join(",\n", dataLayer.Select(i => $"'{i.Key}': '{i.Value}'"));
            var dataLayerScript = string.Format("{0} = [{{\n{1}\n}}];", DataLayerVariableName, values);
            return new HtmlString(dataLayerScript);
        }

        public override HtmlString Print() {

            //this html could be moved to a stand alone file so that you get syntax highlighting etc.
            string tag = string.Format(@"<!-- Google Tag Manager -->
        <script>
            (function(w, d, s, l, i) {{
                w[l] = w[l] || []; w[l].push({{
                    'gtm.start':
                new Date().getTime(), event: 'gtm.js'
            }}); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
            '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        }})(window, document, 'script', '{0}', '{1}');</script>
    <!-- End Google Tag Manager -->", DataLayerVariableName, _containerId);

            return new HtmlString(tag);
        }

        public string DataLayerVariableName { get; set; }

        public override HtmlString PrintNoScript() {
            string tag = @"<!-- Google Tag Manager -->

    <noscript>
        <iframe src='//www.googletagmanager.com/ns.html?id=" + _containerId + @"'
                height='0' width='0' style='display:none;visibility:hidden'></iframe>
    </noscript>
<!-- End Google Tag Manager -->";

            return new HtmlString(tag);
        }

        private readonly string _containerId;
    }
}
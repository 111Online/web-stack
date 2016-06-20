namespace NHS111.Web.Presentation.Analytics {
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class GoogleTagManagerPrinter
        : AnalyticsTagPrinter {

        public GoogleTagManagerPrinter(Dictionary<string, string> data)
            : base(data) {

            if (data == null || !data.ContainsKey("ContainerID"))
                throw new ArgumentException("Cannot construct a GoogleTagManagerPrinter without a ContainerId.");
        }

        public override HtmlString Print() {

            string containerId = _data["ContainerID"];

            //this html could be moved to a stand alone file so that you get syntax highlighting etc.
            string tag = @"<!-- Google Tag Manager -->
    <noscript>
        <iframe src='//www.googletagmanager.com/ns.html?id=" + containerId + @"'
                height='0' width='0' style='display:none;visibility:hidden'></iframe>
        </noscript>
        <script>
            (function(w, d, s, l, i) {
                w[l] = w[l] || []; w[l].push({
                    'gtm.start':
                new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
            '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', '" + containerId + @"');</script>
    <!-- End Google Tag Manager -->";

            return new HtmlString(tag);
        }
    }
}
using System.Web.Mvc;
using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    public class DirectLinkingView<T>
        : WebViewPage<T>
    {
        protected readonly IDirectLinkingFeature DirectLinkingFeature;

        public DirectLinkingView()
        {
            DirectLinkingFeature = new DirectLinkingFeature();
        }

        public override void Execute() { }
    }
}
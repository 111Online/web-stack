
namespace NHS111.Web.Views.Shared
{
    using System.Web.Mvc;
    using Presentation.Features;

    public class DirectLinkingView<T>
        : WebViewPage<T>
    {
        protected readonly IDirectLinkingFeature DirectLinkingFeature;

        public DirectLinkingView() {
            DirectLinkingFeature = new DirectLinkingFeature();
        }

        public override void Execute() { }
    }
}
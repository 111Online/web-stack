using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    public class QuestionView<T>
        : BaseView<T>
    {
        protected readonly IDirectLinkingFeature DirectLinkingFeature;

        public QuestionView()
        {
            DirectLinkingFeature = new DirectLinkingFeature();
        }

        public override void Execute() { }
    }
}
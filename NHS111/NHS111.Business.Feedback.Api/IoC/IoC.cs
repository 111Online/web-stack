using StructureMap;

namespace NHS111.Business.Feedback.Api.IoC
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var cont = new Container(c => c.AddRegistry<FeedbackDomainApiRegistry>());
            //cont.AssertConfigurationIsValid();
            return cont;
        }
    }
}
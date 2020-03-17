using log4net;
using NHS111.Utils.IoC;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;
using RestSharp;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Web.Presentation.IoC
{
    public class WebPresentationRegistry : Registry
    {
        public WebPresentationRegistry()
        {
            IncludeRegistry<UtilsRegistry>();
            For<IRestClient>().Singleton()
                .Use(new LoggingRestClient(new Configuration.Configuration().BusinessApiProtocolandDomain, LogManager.GetLogger("log"))).Named("restClientBusinessApi");
            For<IDOSBuilder>().Singleton()
                .Use<DOSBuilder>()
                .Ctor<IRestClient>()
                .Is(new LoggingRestClient(new Configuration.Configuration().BusinessDosApiBaseUrl, LogManager.GetLogger("log")));
            For<IFeedbackViewModelBuilder>().Singleton()
                .Use<FeedbackViewModelBuilder>()
                .Ctor<IRestClient>()
                .Is(new LoggingRestClient(new Configuration.Configuration().FeedbackApiBaseUrl, LogManager.GetLogger("log")));
            For<IOutcomeViewModelBuilder>().Singleton()
                .Use<OutcomeViewModelBuilder>()
                .Ctor<IRestClient>("restClientPostcodeApi")
                .Is(new LoggingRestClient(new Configuration.Configuration().PostcodeApiBaseUrl, LogManager.GetLogger("log")))
                .Ctor<IRestClient>("restClientItkDispatcherApi")
                .Is(new LoggingRestClient(new Configuration.Configuration().ItkDispatcherApiBaseUrl, LogManager.GetLogger("log")));
            For<ICCGModelBuilder>().Singleton()
                .Use<CCGViewModelBuilder>()
                .Ctor<IRestClient>()
                .Is(new LoggingRestClient(new Configuration.Configuration().CCGBusinessApiBaseProtocolandDomain, LogManager.GetLogger("log")));
            For<IAuditLogger>().Singleton()
                .Use<AuditLogger>()
                .Ctor<IRestClient>()
                .Is(new LoggingRestClient(new Configuration.Configuration().LoggingServiceApiBaseUrl, LogManager.GetLogger("log")));
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
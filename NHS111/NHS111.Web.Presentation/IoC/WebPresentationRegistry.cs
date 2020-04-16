using System.Net;
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
        private Configuration.Configuration _configuration = new Configuration.Configuration();

        public WebPresentationRegistry()
        {
            IncludeRegistry<UtilsRegistry>();

            For<IRestClient>().Singleton()
                .Use(GetLoggingRestClientFor(_configuration.BusinessApiProtocolandDomain)).Named("restClientBusinessApi");
            For<IDOSBuilder>().Singleton()
                .Use<DOSBuilder>()
                .Ctor<IRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.BusinessDosApiBaseUrl));
            For<IFeedbackViewModelBuilder>().Singleton()
                .Use<FeedbackViewModelBuilder>()
                .Ctor<IRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.FeedbackApiBaseUrl));
            For<IOutcomeViewModelBuilder>().Singleton()
                .Use<OutcomeViewModelBuilder>()
                .Ctor<IRestClient>("restClientPostcodeApi")
                .Is(GetLoggingRestClientFor(_configuration.PostcodeApiBaseUrl))
                .Ctor<IRestClient>("restClientItkDispatcherApi")
                .Is(new LoggingRestClient(new Configuration.Configuration().ItkDispatcherApiBaseUrl,
                    LogManager.GetLogger("log")));
            For<IRegisterForSMSViewModelBuilder>().Singleton()
                .Use<RegisterForSMSViewModelBuilder>()
                .Ctor<IRestClient>("restClientCaseDataCaptureApi")
                .Is(GetLoggingRestClientFor(_configuration.CaseDataCaptureApiBaseUrl));
            For<ICCGModelBuilder>().Singleton()
                .Use<CCGViewModelBuilder>()
                .Ctor<IRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.CCGBusinessApiBaseProtocolandDomain));
            For<IAuditLogger>().Singleton()
                .Use<AuditLogger>()
                .Ctor<IRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.LoggingServiceApiBaseUrl));
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private LoggingRestClient GetLoggingRestClientFor(string baseUrl)
        {
            return new LoggingRestClient(baseUrl, LogManager.GetLogger("log"), _configuration.ServicePointManagerDefaultConnectionLimit);
        }
    }
}
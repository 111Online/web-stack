using log4net;
using NHS111.Utils.IoC;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;
using RestSharp;
using StructureMap;
using System.Net;

namespace NHS111.Web.Presentation.IoC
{
    public class WebPresentationRegistry : Registry
    {
        private Configuration.Configuration _configuration = new Configuration.Configuration();

        public WebPresentationRegistry()
        {
            IncludeRegistry<UtilsRegistry>();
            For<IMicroSurveyBuilder>().Singleton()
                .Use<MicroSurveyBuilder>()
                .Ctor<ILoggingRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.QualtricsApiBaseUrl));
            For<ILoggingRestClient>().Singleton()
                .Use(GetLoggingRestClientFor(_configuration.BusinessApiProtocolandDomain)).Named("restClientBusinessApi");
            For<IDOSBuilder>().Singleton()
                .Use<DOSBuilder>()
                .Ctor<ILoggingRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.BusinessDosApiBaseUrl));
            For<IFeedbackViewModelBuilder>().Singleton()
                .Use<FeedbackViewModelBuilder>()
                .Ctor<ILoggingRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.FeedbackApiBaseUrl));
            For<IOutcomeViewModelBuilder>().Singleton()
                .Use<OutcomeViewModelBuilder>()
                .Ctor<ILoggingRestClient>("restClientPostcodeApi")
                .Is(GetLoggingRestClientFor(_configuration.PostcodeApiBaseUrl))
                .Ctor<ILoggingRestClient>("restClientItkDispatcherApi")
                .Is(new LoggingRestClient(new Configuration.Configuration().ItkDispatcherApiBaseUrl,
                    LogManager.GetLogger("log")));
            For<IRegisterForSMSViewModelBuilder>().Singleton()
                .Use<RegisterForSMSViewModelBuilder>()
                .Ctor<ILoggingRestClient>("restClientCaseDataCaptureApi")
                .Is(GetLoggingRestClientFor(_configuration.CaseDataCaptureApiBaseUrl));
            For<ICCGModelBuilder>().Singleton()
                .Use<CCGViewModelBuilder>()
                .Ctor<ILoggingRestClient>()
                .Is(GetLoggingRestClientFor(_configuration.CCGBusinessApiBaseProtocolandDomain));

            if (_configuration.AuditEventHubEnabled)
            {
                For<IAuditLogger>().Singleton()
                    .Use<EventHubAuditLogger>();
            }
            else
            {
                For<IAuditLogger>().Singleton()
                    .Use<AuditLogger>()
                    .Ctor<ILoggingRestClient>()
                    .Is(GetLoggingRestClientFor(new Configuration.Configuration().LoggingServiceApiBaseUrl));
            };

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private LoggingRestClient GetLoggingRestClientFor(string baseUrl)
        {
            return new LoggingRestClient(baseUrl, LogManager.GetLogger("log"), _configuration.ServicePointManagerDefaultConnectionLimit, _configuration.RestClientTimeoutMs);
        }
    }
}

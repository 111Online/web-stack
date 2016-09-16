
namespace NHS111.Logging.Api.Unit.Test {
    using System;
    using System.Threading.Tasks;
    using Controllers;
    using log4net;
    using log4net.Config;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using Moq;
    using NHS111.Models.Models.Web.Logging;
    using NUnit.Framework;
    using Utils.Logging;

    [TestFixture]
    public class LoggingApiTest {
        private static readonly Mock<ILogServiceContext> _mockLogServiceContext = new Mock<ILogServiceContext>();

        [Test]
        public async Task Audit_Always_StoresAuditInAzureStorage() {

            ConfigureLog4Net();

            var logsController = new LogsController();
            var audit = new AuditEntry {
                SessionId = Guid.NewGuid(),
                PathwayId = "PW118",
                PathwayTitle = "Test",
                Journey = "{ some: 'thing' }",
                State = "{ someOther: 'thingy' }"
            };

            await logsController.Audit(audit);

            _mockLogServiceContext.Verify(c => c.Log(It.Is<AuditEntry>(a => a.SessionId == audit.SessionId)));
        }

        private static void ConfigureLog4Net() {
            BasicConfigurator.Configure();

            var logRepository = ((Hierarchy) LogManager.GetRepository());
            var root = logRepository.Root;
            var attachable = (IAppenderAttachable) root;

            var appender = new AzureTableStorageAppender(_mockLogServiceContext.Object);
            if (attachable != null)
                attachable.AddAppender(appender);

            logRepository.Threshold = LogAudit.AuditLevel;
            logRepository.RaiseConfigurationChanged(EventArgs.Empty);
        }
    }
}
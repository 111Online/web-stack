
namespace NHS111.Logging.Api.Unit.Test {
    using System;
    using System.Data.Services.Client;
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

        [Test]
        public async Task Audit_Always_StoresAuditInAzureStorage() {
            await _logsController.Audit(_audit);

            _mockLogServiceContext.Verify(c => c.Log(It.Is<AuditEntry>(a => a.SessionId == _audit.SessionId)));
        }

        [Test]
        public void ActivateOptions_WithConcreteLogServiceContext_ThrowsDataServiceRequestException() {
            var appender = new AzureTableStorageAppender {
                TableStorageAccountName = "someaccount",
                TableStorageAccountKey =
                    Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("somekey".ToCharArray())),
                TableStorageName = "sometable"
            };

            Assert.Throws(Is.InstanceOf<Exception>(), delegate { appender.ActivateOptions(); });
        }

        [SetUp]
        public void SetUp() {
            BasicConfigurator.Configure();

            var logRepository = ((Hierarchy) LogManager.GetRepository());
            var root = logRepository.Root;
            var attachable = (IAppenderAttachable) root;

            _appender = new AzureTableStorageAppender(_mockLogServiceContext.Object);
            if (attachable != null)
                attachable.AddAppender(_appender);

            logRepository.Threshold = LogAudit.AuditLevel;
            logRepository.RaiseConfigurationChanged(EventArgs.Empty);

            _audit = new AuditEntry {
                SessionId = Guid.NewGuid(),
                PathwayId = "PW118",
                PathwayTitle = "Test",
                Journey = "{ some: 'thing' }",
                State = "{ someOther: 'thingy' }"
            };

        }

        private static readonly Mock<ILogServiceContext> _mockLogServiceContext = new Mock<ILogServiceContext>();
        private readonly LogsController _logsController = new LogsController();
        private AuditEntry _audit;
        private AzureTableStorageAppender _appender;
    }
}
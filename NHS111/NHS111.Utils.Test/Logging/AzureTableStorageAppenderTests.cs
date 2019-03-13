using log4net.Core;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.Logging;
using NHS111.Utils.Logging;
using NUnit.Framework;

namespace NHS111.Utils.Test.Logging
{
    [TestFixture]
    public class AzureTableStorageAppenderTests
    {
        private Mock<ILogServiceContext> _mockLogServiceContext;

        [SetUp]
        public void SetUp()
        {
            _mockLogServiceContext = new Mock<ILogServiceContext>();
        }

        [Test]
        public void Append_WithValidLogEntry_CallsLog()
        {
            var appender = new AzureTableStorageAppender(_mockLogServiceContext.Object);
            appender.DoAppend(new LoggingEvent(new LoggingEventData() { Message = JsonConvert.SerializeObject(new AuditEntry()) }));
            _mockLogServiceContext.Verify(x => x.Log(It.IsAny<AuditEntry>()), Times.Once);
        }

        [Test]
        public void Append_WithInvalidLogEntry_NoLog()
        {
            var appender = new AzureTableStorageAppender(_mockLogServiceContext.Object);
            appender.DoAppend(new LoggingEvent(new LoggingEventData() { Message = It.IsAny<string>() }));
            _mockLogServiceContext.Verify(x => x.Log(It.IsAny<AuditEntry>()), Times.Never);
        }
    }
}

using Moq;
using NHS111.Domain.Api.Controllers;
using NHS111.Utils.Monitoring;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NHS111.Domain.Test.API_Project.Controller
{
    [TestFixture]
    public class MonitoringController_Test
    {
        private Mock<IMonitor> _monitor;
        private MonitoringController _sut;

        [SetUp]
        public void SetUp()
        {
            _monitor = new Mock<IMonitor>();
            _sut = new MonitoringController(_monitor.Object);
        }

        [Test]
        public async void should_return_a_monitoring_string_service_ping()
        {
            //Arrange
            var service = "ping";
            var expectedResult = "pong";

            _monitor.Setup(x => x.Ping()).Returns(expectedResult);

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Ping(), Times.Once);
            Assert.That(result, Is.EqualTo(expectedResult));

        }


        [Test]
        public async void should_return_a_monitoring_string_service_ping_written_differently()
        {
            //Arrange
            var service = " pInG ";
            var expectedResult = "pong";

            _monitor.Setup(x => x.Ping()).Returns(expectedResult);

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Ping(), Times.Once);
            Assert.That(result, Is.EqualTo(expectedResult));

        }

        [Test]
        public async void should_return_a_mocked_monitoring_string_service_metrics()
        {
            //Arrange
            var service = "metrics";
            var expectedResult = "Metrics";

            _monitor.Setup(x => x.Metrics()).Returns(expectedResult);

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Metrics(), Times.Once);

            Assert.That(result, Is.EqualTo(expectedResult));

        }

        [Test]
        public async void should_return_a_monitoring_string_service_metrics_written_differently()
        {
            //Arrange
            var service = " mEtRiCs ";
            var expectedResult = "Metrics";

            _monitor.Setup(x => x.Metrics()).Returns(expectedResult);

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Metrics(), Times.Once);

            Assert.That(result, Is.EqualTo(expectedResult));

        }

        [Test]
        public async void should_return_a_monitoring_string_service_health()
        {
            //Arrange
            var service = "health";
            var expectedResult = true;

            _monitor.Setup(x => x.Health()).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Health(), Times.Once);
            Assert.That(Convert.ToBoolean(result), Is.EqualTo(expectedResult));

        }

        [Test]
        public async void should_return_a_monitoring_string_service_health_written_differently()
        {
            //Arrange
            var service = " hEaLtH ";
            var expectedResult = true;

            _monitor.Setup(x => x.Health()).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Health(), Times.Once);
            Assert.That(Convert.ToBoolean(result), Is.EqualTo(expectedResult));

        }

        [Test]
        public async void should_return_null()
        {

            //Arrange
            var service = "thiscasedoesnotexist";

            //Act
            var result = await _sut.MonitorPing(service);

            //Assert
            _monitor.Verify(x => x.Ping(), Times.Never);
            _monitor.Verify(x => x.Metrics(), Times.Never);
            _monitor.Verify(x => x.Health(), Times.Never);
            Assert.That(result, Is.EqualTo(null));
        }
    }
}
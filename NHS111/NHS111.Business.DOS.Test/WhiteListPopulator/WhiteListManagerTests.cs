using Moq;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.DispositionMapper;
using NHS111.Business.DOS.WhiteListPopulator;
using NHS111.Utils.RestTools;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.DOS.Test.WhiteListPopulator
{
    [TestFixture]
    public class WhiteListManagerTests
    {
        private Mock<IDispositionMapper> _mockDispositionMapper;
        private Mock<ILoggingRestClient> _mockRestClient;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp]
        public void Setup()
        {
            _mockDispositionMapper = new Mock<IDispositionMapper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockRestClient = new Mock<ILoggingRestClient>();
        }

        [Test]
        public void WhiteListManager_ReturnsPharmacyForPharmacyDx()
        {
            int dxCode = 1085;
            _mockDispositionMapper.Setup(d => d.IsRepeatPrescriptionDisposition(dxCode)).Returns(true);
            var sut = new WhiteListManager(_mockDispositionMapper.Object, _mockRestClient.Object, _mockConfiguration.Object);
            var result = sut.GetWhiteListPopulator(dxCode);
            Assert.IsInstanceOf<PharmacyReferralServicesWhiteListPopulator>(result);
        }

        [Test]
        public void WhiteListManager_ReturnsStandardReferralForNonPharmacyDx()
        {
            int dxCode = 1011;
            _mockDispositionMapper.Setup(d => d.IsGenericDisposition(dxCode)).Returns(true);
            var sut = new WhiteListManager(_mockDispositionMapper.Object, _mockRestClient.Object, _mockConfiguration.Object);
            var result = sut.GetWhiteListPopulator(dxCode);
            Assert.IsInstanceOf<ReferralServicesWhiteListPopulator>(result);
        }


        [Test]
        public void WhiteListManager_ReturnsStandardReferralForUnknownDx()
        {
            int dxCode = 12345;
            var sut = new WhiteListManager(_mockDispositionMapper.Object, _mockRestClient.Object, _mockConfiguration.Object);
            var result = sut.GetWhiteListPopulator(dxCode);
            Assert.IsInstanceOf<ReferralServicesWhiteListPopulator>(result);
        }
    }
}
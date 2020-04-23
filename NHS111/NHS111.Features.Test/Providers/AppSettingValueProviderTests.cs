using Moq;
using NHS111.Features.Defaults;
using NHS111.Features.Providers;
using NUnit.Framework;
using System.Configuration;

namespace NHS111.Features.Test.Providers
{
    [TestFixture]
    public class AppSettingValueProviderTests
    {

        [Test]
        [Ignore("Can't isolate configuration manager.")]
        public void GetSetting_WithValue_ReturnsValue()
        {

            var sut = new AppSettingValueProvider();
            var feature = new Mock<IFeature>().Object;

            ConfigurationManager.AppSettings[feature.GetType().Name] = "true";
            var result = sut.GetSetting(feature, null, It.IsAny<string>());
            Assert.AreEqual("true", result.Value);

            ConfigurationManager.AppSettings[feature.GetType().Name] = "false";
            result = sut.GetSetting(feature, null, It.IsAny<string>());
            Assert.AreEqual("false", result.Value);

            ConfigurationManager.AppSettings.Remove(feature.GetType().Name);
        }

        [Test]
        [ExpectedException(typeof(MissingSettingException))]
        public void GetSetting_WithNullDefaultBoolStrategy_ThrowsException()
        {

            var sut = new AppSettingValueProvider();
            sut.GetSetting(new Mock<IFeature>().Object, null, It.IsAny<string>());
        }

        [Test]
        public void IsEnabled_WithDefaultBoolStrategy_QueriesDefaultStrategy()
        {
            var sut = new AppSettingValueProvider();
            var defaultSettingStrategy = new Mock<IDefaultSettingStrategy>();
            sut.GetSetting(new Mock<IFeature>().Object, defaultSettingStrategy.Object, It.IsAny<string>());

            defaultSettingStrategy.Verify(s => s.Value);
        }
    }
}
using Moq;
using NHS111.Features.Defaults;
using NHS111.Features.Providers;
using NHS111.Features.Values;
using NUnit.Framework;
using System.Configuration;

namespace NHS111.Features.Test
{
    [TestFixture]
    public class BaseFeatureTests
    {

        private class TestFeature : BaseFeature { }

        [Test]
        [Ignore("Can't isolate configuration manager.")]
        public void IsEnabled_WithDefaultProvider_QueriesDefaultProvider()
        {
            var basicFeature = new TestFeature();

            ConfigurationManager.AppSettings["TestFeature"] = "true";
            Assert.IsTrue(basicFeature.IsEnabled);

            ConfigurationManager.AppSettings["TestFeature"] = "false";
            Assert.IsFalse(basicFeature.IsEnabled);

            //ConfigurationManager.AppSettings.Remove("TestFeature");
        }

        [Test]
        public void IsEnabled_WithProvider_QueriesSuppliedProvider()
        {
            var mockProvider = new Mock<IFeatureSettingValueProvider>();
            var basicFeature = new TestFeature { SettingValueProvider = mockProvider.Object };
            mockProvider.Setup(
               p => p.GetSetting(It.Is<IFeature>(f => f == basicFeature), It.IsAny<IDefaultSettingStrategy>(), It.IsAny<string>()))
               .Returns(new FeatureValue(string.Empty));

            var result = basicFeature.IsEnabled;

            mockProvider.Verify(p => p.GetSetting(It.Is<IFeature>(f => f == basicFeature), It.IsAny<IDefaultSettingStrategy>(), It.IsAny<string>()));
        }

        [Test]
        public void IsEnabled_Always_PassesDefaultSettingStrategyIntoValueProvider()
        {
            var mockProvider = new Mock<IFeatureSettingValueProvider>();
            var mockStrategy = new Mock<IDefaultSettingStrategy>();
            var basicFeature = new TestFeature { SettingValueProvider = mockProvider.Object, DefaultIsEnabledSettingStrategy = mockStrategy.Object };

            mockProvider.Setup(
               p => p.GetSetting(It.IsAny<IFeature>(), It.Is<IDefaultSettingStrategy>(s => s == mockStrategy.Object), It.IsAny<string>()))
               .Returns(new FeatureValue(string.Empty));

            var result = basicFeature.IsEnabled;

            mockProvider.Verify(p => p.GetSetting(It.IsAny<IFeature>(), It.Is<IDefaultSettingStrategy>(s => s == mockStrategy.Object), It.IsAny<string>()));
        }
    }
}

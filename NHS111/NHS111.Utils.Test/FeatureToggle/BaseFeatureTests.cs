
namespace NHS111.Utils.Test.FeatureToggle {

    using System.Configuration;
    using Moq;
    using NUnit.Framework;
    using Utils.FeatureToggle;

    [TestFixture]
    public class BaseFeatureTests {

        private class TestFeature
            : BaseFeature { }

        [Test]
        [Ignore("Can't isolate configuration manager.")]
        public void IsEnabled_WithDefaultProvider_QueriesDefaultProvider() {
            var basicFeature = new TestFeature();

            ConfigurationManager.AppSettings["TestFeature"] = "true";
            Assert.IsTrue(basicFeature.IsEnabled);

            ConfigurationManager.AppSettings["TestFeature"] = "false";
            Assert.IsFalse(basicFeature.IsEnabled);

            //ConfigurationManager.AppSettings.Remove("TestFeature");
        }

        [Test]
        public void IsEnabled_WithProvider_QueriesSuppliedBoolProvider() {
            var mockProvider = new Mock<IFeatureSettingValueProvider<bool>>();
            var basicFeature = new TestFeature {BoolSettingValueProvider = mockProvider.Object};

            var result = basicFeature.IsEnabled;

            mockProvider.Verify(p => p.GetSetting(It.Is<IFeature>(f => f == basicFeature), It.IsAny<IDefaultSettingStrategy<bool>>()));
        }

        [Test]
        public void IsEnabled_Always_PassesDefaultBoolSettingStrategyIntoValueProvider() {
            var mockProvider = new Mock<IFeatureSettingValueProvider<bool>>();
            var mockStrategy = new Mock<IDefaultSettingStrategy<bool>>();
            var basicFeature = new TestFeature { BoolSettingValueProvider = mockProvider.Object, DefaultBoolSettingStrategy = mockStrategy.Object };

            var result = basicFeature.IsEnabled;

            mockProvider.Verify(p => p.GetSetting(It.IsAny<IFeature>(), It.Is<IDefaultSettingStrategy<bool>>(s => s == mockStrategy.Object)));
        }

        [Test]
        public void IsEnabled_WithProvider_QueriesSuppliedStringProvider()
        {
            var mockProvider = new Mock<IFeatureSettingValueProvider<string>>();
            var basicFeature = new TestFeature { StringSettingValueProvider = mockProvider.Object };

            var result = basicFeature.StringValue;

            mockProvider.Verify(p => p.GetSetting(It.Is<IFeature>(f => f == basicFeature), It.IsAny<IDefaultSettingStrategy<string>>()));
        }

        [Test]
        public void IsEnabled_Always_PassesDefaultStringSettingStrategyIntoValueProvider()
        {
            var mockProvider = new Mock<IFeatureSettingValueProvider<string>>();
            var mockStrategy = new Mock<IDefaultSettingStrategy<string>>();
            var basicFeature = new TestFeature { StringSettingValueProvider = mockProvider.Object, DefaultStringSettingStrategy = mockStrategy.Object };

            var result = basicFeature.StringValue;

            mockProvider.Verify(p => p.GetSetting(It.IsAny<IFeature>(), It.Is<IDefaultSettingStrategy<string>>(s => s == mockStrategy.Object)));
        }
    }
}

namespace NHS111.Utils.Test.FeatureToggle {
    using System.Configuration;
    using Moq;
    using NUnit.Framework;
    using Utils.FeatureToggle;

    [TestFixture]
    public class AppSettingValueProviderTests {

        [Test]
        [Ignore("Can't isolate configuration manager.")]
        public void GetSetting_WithValue_ReturnsValue() {

            var sut = new AppSettingValueProvider();
            var feature = new Mock<IFeature>().Object;

            ConfigurationManager.AppSettings[feature.GetType().Name] = "true";
            var result = sut.GetSetting(feature, null);
            Assert.IsTrue(result);

            ConfigurationManager.AppSettings[feature.GetType().Name] = "false";
            result = sut.GetSetting(feature, null);
            Assert.IsFalse(result);

            ConfigurationManager.AppSettings.Remove(feature.GetType().Name);
        }

        [Test]
        [ExpectedException(typeof (MissingSettingException))]
        public void GetSetting_WithNullDefaultStrategy_ThrowsException() {

            var sut = new AppSettingValueProvider();
            sut.GetSetting(new Mock<IFeature>().Object, null);
        }

        [Test]
        public void IsEnabled_WithDefaultStrategy_QueriesDefaultStrategy() {
            var sut = new AppSettingValueProvider();
            var defaultSettingStrategy = new Mock<IDefaultSettingStrategy>();
            sut.GetSetting(new Mock<IFeature>().Object, defaultSettingStrategy.Object);

            defaultSettingStrategy.Verify(s => s.GetDefaultSetting());
        }
    }
}
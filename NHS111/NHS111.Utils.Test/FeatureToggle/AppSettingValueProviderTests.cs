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

            var sut = new AppSettingBoolValueProvider();
            var feature = new Mock<IFeature>().Object;

            ConfigurationManager.AppSettings[feature.GetType().Name] = "true";
            var result = sut.GetSetting(feature, null, "IsEnabled");
            Assert.IsTrue(result);

            ConfigurationManager.AppSettings[feature.GetType().Name] = "false";
            result = sut.GetSetting(feature, null, "IsEnabled");
            Assert.IsFalse(result);

            ConfigurationManager.AppSettings.Remove(feature.GetType().Name);
        }

        [Test]
        [ExpectedException(typeof (MissingSettingException))]
        public void GetSetting_WithNullDefaultBoolStrategy_ThrowsException() {

            var sut = new AppSettingBoolValueProvider();
            sut.GetSetting(new Mock<IFeature>().Object, null, It.IsAny<string>());
        }

        [Test]
        public void IsEnabled_WithDefaultBoolStrategy_QueriesDefaultStrategy() {
            var sut = new AppSettingBoolValueProvider();
            var defaultSettingStrategy = new Mock<IDefaultSettingStrategy<bool>>();
            sut.GetSetting(new Mock<IFeature>().Object, defaultSettingStrategy.Object, It.IsAny<string>());

            defaultSettingStrategy.Verify(s => s.GetDefaultSetting());
        }

        [Test]
        [ExpectedException(typeof(MissingSettingException))]
        public void GetSetting_WithNullDefaultStringStrategy_ThrowsException()
        {

            var sut = new AppSettingStringValueProvider();
            sut.GetSetting(new Mock<IFeature>().Object, null, It.IsAny<string>());
        }

        [Test]
        public void IsEnabled_WithDefaultStringStrategy_QueriesDefaultStrategy()
        {
            var sut = new AppSettingStringValueProvider();
            var defaultSettingStrategy = new Mock<IDefaultSettingStrategy<string>>();
            sut.GetSetting(new Mock<IFeature>().Object, defaultSettingStrategy.Object, It.IsAny<string>());

            defaultSettingStrategy.Verify(s => s.GetDefaultSetting());
        }
    }
}
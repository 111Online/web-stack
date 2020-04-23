using Moq;
using NHS111.Features.Defaults;
using NHS111.Features.Providers;
using NHS111.Features.Values;
using NUnit.Framework;
using System.IO;

namespace NHS111.Features.Test
{
    [TestFixture]
    public class AllowedPostcodeFeatureTests
    {
        [Test]
        public void Is_enabled_returns_true_by_default()
        {
            var sut = new AllowedPostcodeFeature();
            Assert.IsTrue(sut.IsEnabled);
        }

        [Test]
        public void Postcode_file_missing_file_path_returns_null_textreader()
        {
            var sut = new AllowedPostcodeFeature();
            Assert.AreEqual(TextReader.Null, sut.PostcodeFile);
        }

        [Test]
        public void Feature_enabled_missing_directory_returns_false()
        {
            var mockFeature = new Mock<IFeatureSettingValueProvider>();
            mockFeature.Setup(f => f.GetSetting(It.IsAny<IFeature>(), It.IsAny<IDefaultSettingStrategy>(), It.IsAny<string>())).Returns(new FeatureValue(@"missing/file/path"));

            var sut = new AllowedPostcodeFeature();
            Assert.AreEqual(TextReader.Null, sut.PostcodeFile);
        }

        [Test]
        public void Feature_enabled_missing_file_returns_false()
        {
            var mockFeature = new Mock<IFeatureSettingValueProvider>();
            mockFeature.Setup(f => f.GetSetting(It.IsAny<IFeature>(), It.IsAny<IDefaultSettingStrategy>(), It.IsAny<string>())).Returns(new FeatureValue(@"missing/file/path/file.csv"));

            var sut = new AllowedPostcodeFeature();
            Assert.AreEqual(TextReader.Null, sut.PostcodeFile);
        }
    }
}

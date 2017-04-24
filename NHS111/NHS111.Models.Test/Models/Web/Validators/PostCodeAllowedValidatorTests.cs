using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Features;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class PostCodeAllowedValidatorTests
    {
        [Test]
        public void Feature_not_enabled_returns_is_valid_true()
        {
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(false);

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsTrue(sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_missing_file_path_returns_false()
        {
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns((TextReader.Null));

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsFalse(sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_empty_postcode_list_returns_false()
        {
            var mockPostcodeList = @"Postcode";

            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(mockPostcodeList));

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsFalse(sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_allowed_postcode_space_returns_true()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };

            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsTrue(sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_allowed_postcode_no_space_returns_true()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };

            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsTrue(sut.IsAllowedPostcode("SO302UN"));
        }

        [Test]
        public void Feature_enabled_postcode_case_insensitive_returns_true()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };

            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsTrue(sut.IsAllowedPostcode("So30 2uN"));
        }

        [Test]
        public void Feature_enabled_postcode_not_allowed_returns_false()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };

            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object);
            Assert.IsFalse(sut.IsAllowedPostcode("Ls1 6Xy"));
        }
    }
}

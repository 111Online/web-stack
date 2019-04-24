using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Features;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Validators;
using NUnit.Framework;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]

    public class PostCodeAllowedValidatorTests
    {
        Mock<ICCGModelBuilder> mockCCGBuilder = new Mock<ICCGModelBuilder>();
      
        public void SetupMockCCGResultWithApp()
        {
            mockCCGBuilder.Setup(f => f.FillCCGModel(It.IsAny<string>())).ReturnsAsync(new CCGModel() { App = "Pathways", Postcode = "TS19 7TG"});
        }

        public void SetupMockCCGResultWithoutValidPostcode()
        {
            mockCCGBuilder.Setup(f => f.FillCCGModel(It.IsAny<string>())).ReturnsAsync(new CCGModel() );
        }

        [Test]
        public void Feature_not_enabled_returns_is_valid_true()
        {
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(false);
            SetupMockCCGResultWithApp();
            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysArea, sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_empty_postcode_list_returns_false()
        {
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            SetupMockCCGResultWithoutValidPostcode();
            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.PostcodeNotFound, sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_allowed_postcode_space_returns_true()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };
            SetupMockCCGResultWithApp();
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysArea, sut.IsAllowedPostcode("SO30 2UN"));
        }

        [Test]
        public void Feature_enabled_allowed_postcode_no_space_returns_true()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };
            SetupMockCCGResultWithApp();
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysArea, sut.IsAllowedPostcode("SO302UN"));
        }

        [Test]
        public void Feature_enabled_postcode_case_insensitive_returns_true()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };
            SetupMockCCGResultWithApp();
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysArea, sut.IsAllowedPostcode("So30 2uN"));
        }
    }
}

﻿using Moq;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Validators;
using NUnit.Framework;
using System;
using System.IO;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]

    public class PostCodeAllowedValidatorTests
    {
        Mock<ICCGModelBuilder> mockCCGBuilder = new Mock<ICCGModelBuilder>();

        public void SetupMockCCGResultWithApp()
        {
            mockCCGBuilder.Setup(f => f.FillCCGDetailsModelAsync(It.IsAny<string>())).ReturnsAsync(new CCGDetailsModel() { App = "Pathways", Postcode = "TS19 7TG", PharmacyReferralServiceIdWhitelist = new ServiceListModel() });
        }

        public void SetupMockCCGResultWithoutValidPostcode()
        {
            mockCCGBuilder.Setup(f => f.FillCCGDetailsModelAsync(It.IsAny<string>())).ReturnsAsync(new CCGDetailsModel());
        }

        [Test]
        public void Feature_not_enabled_returns_is_valid_true()
        {
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(false);
            SetupMockCCGResultWithApp();
            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices, sut.IsAllowedPostcode("SO30 2UN"));
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
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices, sut.IsAllowedPostcode("SO30 2UN"));
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
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices, sut.IsAllowedPostcode("SO302UN"));
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
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices, sut.IsAllowedPostcode("So30 2uN"));
        }

        [Test]
        public void EP_enabled_area_returns_pathway_with_pharmacy_area()
        {
            var mockPostcodeList = new[] { "Postcode", "SO30 2UN" };
            mockCCGBuilder.Setup(f => f.FillCCGDetailsModelAsync(It.IsAny<string>())).ReturnsAsync(new CCGDetailsModel() { App = "Pathways", Postcode = "TS19 7TG", PharmacyReferralServiceIdWhitelist = new ServiceListModel("123|456") });
            var mockFeature = new Mock<IAllowedPostcodeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.PostcodeFile).Returns(new StringReader(string.Join(Environment.NewLine, mockPostcodeList)));

            var sut = new PostCodeAllowedValidator(mockFeature.Object, mockCCGBuilder.Object);
            Assert.AreEqual(PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices, sut.IsAllowedPostcode("So30 2uN"));
        }

        [Test]
        public void Outcome_pathway_with_pharmacy_area_returns_in_area_true()
        {
            Assert.IsTrue(PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices.IsInAreaForOutcome());
        }

        [Test]
        public void Outcome_pathway_without_pharmacy_area_returns_in_area_true()
        {
            Assert.IsTrue(PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices.IsInAreaForOutcome());
        }
    }
}

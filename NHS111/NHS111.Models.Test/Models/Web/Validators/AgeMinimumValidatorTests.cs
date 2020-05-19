using Moq;
using NHS111.Features;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;
using System;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class AgeMinimumValidatorTests
    {
        [Test]
        public void Feature_not_enabled_returns_is_valid_true()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(false);

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsTrue(sut.IsAValidAge(24));
        }

        [Test]
        public void Feature_enabled_empty_age_list_returns_is_valid_true()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.FilteredAgeCategories).Returns(new string[0]);

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsTrue(sut.IsAValidAge(24));
        }

        [Test]
        public void Feature_enabled_exclude_infants_and_toddlers_age_4_returns_false()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.FilteredAgeCategories).Returns(new[] { "infant", "toddler" });

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsFalse(sut.IsAValidAge(4));
        }

        [Test]
        public void Feature_enabled_exclude_infants_and_toddlers_age_1_returns_false()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.FilteredAgeCategories).Returns(new[] { "infant", "toddler" });

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsFalse(sut.IsAValidAge(1));
        }

        [Test]
        public void Feature_enabled_exclude_infants_and_toddlers_age_0_returns_false()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.FilteredAgeCategories).Returns(new[] { "infant", "toddler" });

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsFalse(sut.IsAValidAge(0));
        }

        [Test]
        public void Feature_enabled_exclude_infants_and_toddlers_age_5_returns_true()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.FilteredAgeCategories).Returns(new[] { "infant", "toddler" });

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsTrue(sut.IsAValidAge(5));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Feature_enabled_invalid_age_categories_throws_exception()
        {
            var mockFeature = new Mock<IFilterPathwaysByAgeFeature>();
            mockFeature.Setup(f => f.IsEnabled).Returns(true);
            mockFeature.Setup(f => f.FilteredAgeCategories).Returns(new[] { "x", "toddler" });

            var sut = new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age, mockFeature.Object);
            Assert.IsTrue(sut.IsAValidAge(5));
        }
    }
}

using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class AgeMaximumValidatorTests
    {
        [Test]
        public void AgeMaximumValidator_Age_Infant_Minimum_returns_true()
        {
            var sut = new AgeMaximumValidator<AgeGenderViewModel, int>(u => u.Age);
            Assert.IsTrue(sut.IsAValidAge(AgeCategory.Infant.MinimumAge));
        }

        [Test]
        public void AgeMaximumValidator_Age_Adult_Maximum_returns_true()
        {
            var sut = new AgeMaximumValidator<AgeGenderViewModel, int>(u => u.Age);
            Assert.IsTrue(sut.IsAValidAge(AgeCategory.Adult.MaximumAge));
        }

        [Test]
        public void AgeMaximumValidator_Age_Over_Adult_Maximum_returns_false()
        {
            var sut = new AgeMaximumValidator<AgeGenderViewModel, int>(u => u.Age);
            Assert.IsFalse(sut.IsAValidAge(AgeCategory.Adult.MaximumAge + 1));
        }
    }
}

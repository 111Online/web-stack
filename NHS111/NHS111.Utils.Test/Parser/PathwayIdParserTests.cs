namespace NHS111.Utils.Test.Parser
{
    using Models.Models.Domain;
    using NUnit.Framework;
    using Utils.Parser;

    [TestFixture]
    public class PathwayIdParserTests
    {

        [Test]
        public void TryParse_WithCorrectFormat_ReturnsCorrectAgeAndGender()
        {
            var number = "PW999MaleAdult";

            string pathwayId;
            AgeCategory age;
            Gender gender;

            var result = PathwayIdParser.TryParse(number, out pathwayId, out gender, out age);
            Assert.IsTrue(result);
            Assert.IsTrue(pathwayId == "PW999");
            Assert.IsTrue(gender.Equals(Gender.Male));
            Assert.IsTrue(age.Equals(AgeCategory.Adult));
        }

        [Test]
        public void TryParse_WithBadAge_ReturnsFalse()
        {
            var number = "PW999MaleBlah";

            string pathwayId;
            AgeCategory age;
            Gender gender;

            var result = PathwayIdParser.TryParse(number, out pathwayId, out gender, out age);
            Assert.IsFalse(result);
        }

        [Test]
        public void TryParse_WithBadGender_ReturnsFalse()
        {
            var number = "PW999BlahAdult";

            string pathwayId;
            AgeCategory age;
            Gender gender;

            var result = PathwayIdParser.TryParse(number, out pathwayId, out gender, out age);
            Assert.IsFalse(result);
        }

    }
}
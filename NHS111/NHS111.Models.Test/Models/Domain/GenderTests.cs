
namespace NHS111.Models.Test.Models.Domain
{
    using NHS111.Models.Models.Domain;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class GenderTests
    {

        [Test]
        public void Ctor_WithValidStrings_CreatesCorrectObject()
        {
            var sut = new Gender("Male");
            Assert.AreEqual("Male", sut.Value);
            sut = new Gender("M");
            Assert.AreEqual("Male", sut.Value);
            sut = new Gender("male");
            Assert.AreEqual("Male", sut.Value);
            sut = new Gender("m");
            Assert.AreEqual("Male", sut.Value);
            sut = new Gender("Female");
            Assert.AreEqual("Female", sut.Value);
            sut = new Gender("F");
            Assert.AreEqual("Female", sut.Value);
            sut = new Gender("female");
            Assert.AreEqual("Female", sut.Value);
            sut = new Gender("f");
            Assert.AreEqual("Female", sut.Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_WithInvalidString_ThrowsArgumentException()
        {
            new Gender("SomethingInvalid");
        }

        [Test]
        public void Ctor_WithEnum_CreatesCorrectObject()
        {
            var sut = new Gender(GenderEnum.Male);
            Assert.AreEqual("Male", sut.Value);
            sut = new Gender(GenderEnum.Female);
            Assert.AreEqual("Female", sut.Value);
        }
    }
}

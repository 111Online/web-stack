
namespace NHS111.Models.Test.Models.Domain {
    using System;
    using NHS111.Models.Models.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class AgeCategoryTests {

        [Test]
        public void Ctor_WithValidInt_CreatsValidObject() {
            var sut = new AgeCategory(0);
            Assert.AreEqual("Infant", sut.Value);
            sut = new AgeCategory(1);
            Assert.AreEqual("Toddler", sut.Value);
            sut = new AgeCategory(5);
            Assert.AreEqual("Child", sut.Value);
            sut = new AgeCategory(16);
            Assert.AreEqual("Adult", sut.Value);
        }

        [Test]
        public void Ctor_WithValidString_CreatesValidObject() {
            var sut = new AgeCategory("Infant");
            Assert.AreEqual("Infant", sut.Value);
            sut = new AgeCategory("I");
            Assert.AreEqual("Infant", sut.Value);
            sut = new AgeCategory("infant");
            Assert.AreEqual("Infant", sut.Value);
            sut = new AgeCategory("i");
            Assert.AreEqual("Infant", sut.Value);

            sut = new AgeCategory("Toddler");
            Assert.AreEqual("Toddler", sut.Value);
            sut = new AgeCategory("T");
            Assert.AreEqual("Toddler", sut.Value);
            sut = new AgeCategory("toddler");
            Assert.AreEqual("Toddler", sut.Value);
            sut = new AgeCategory("t");
            Assert.AreEqual("Toddler", sut.Value);

            sut = new AgeCategory("Child");
            Assert.AreEqual("Child", sut.Value);
            sut = new AgeCategory("C");
            Assert.AreEqual("Child", sut.Value);
            sut = new AgeCategory("child");
            Assert.AreEqual("Child", sut.Value);
            sut = new AgeCategory("c");
            Assert.AreEqual("Child", sut.Value);

            sut = new AgeCategory("Adult");
            Assert.AreEqual("Adult", sut.Value);
            sut = new AgeCategory("A");
            Assert.AreEqual("Adult", sut.Value);
            sut = new AgeCategory("adult");
            Assert.AreEqual("Adult", sut.Value);
            sut = new AgeCategory("a");
            Assert.AreEqual("Adult", sut.Value);
        }

        [Test]
        public void Ctor_WithEnum_CreatesValidObject() {
            var sut = new AgeCategory(AgeCategoryEnum.Infant);
            Assert.AreEqual("Infant", sut.Value);
            sut = new AgeCategory(AgeCategoryEnum.Toddler);
            Assert.AreEqual("Toddler", sut.Value);
            sut = new AgeCategory(AgeCategoryEnum.Child);
            Assert.AreEqual("Child", sut.Value);
            sut = new AgeCategory(AgeCategoryEnum.Adult);
            Assert.AreEqual("Adult", sut.Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_WithInvalidString_ThrowsArgumentException() {
            new AgeCategory("SomeNonsense");
        }

        [Test]
        public void Infant_agecategory_return_correct_minimum_and_maximum_ages()
        {
            var sut = new AgeCategory(AgeCategoryEnum.Infant);
            Assert.AreEqual(0, sut.MinimumAge);
            Assert.AreEqual(0, sut.MaximumAge);
        }

        [Test]
        public void Toddler_agecategory_return_correct_minimum_and_maximum_ages()
        {
            var sut = new AgeCategory(AgeCategoryEnum.Toddler);
            Assert.AreEqual(1, sut.MinimumAge);
            Assert.AreEqual(4, sut.MaximumAge);
        }

        [Test]
        public void Child_agecategory_return_correct_minimum_and_maximum_ages()
        {
            var sut = new AgeCategory(AgeCategoryEnum.Child);
            Assert.AreEqual(5, sut.MinimumAge);
            Assert.AreEqual(15, sut.MaximumAge);
        }

        [Test]
        public void Adult_agecategory_return_correct_minimum_and_maximum_ages()
        {
            var sut = new AgeCategory(AgeCategoryEnum.Adult);
            Assert.AreEqual(16, sut.MinimumAge);
            Assert.AreEqual(119, sut.MaximumAge);
        }
    }
}
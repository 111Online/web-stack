
namespace NHS111.Models.Test.Models.Domain
{
    using NHS111.Models.Models.Domain;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DispositionCodeTests
    {

        [Test]
        public void Ctor_WithValidArg_CreatesCorrectObject()
        {
            var dxCode = "Dx666";
            var sut = new DispositionCode(dxCode);
            Assert.AreEqual(dxCode, sut.Value);
        }

        [Test]
        public void Ctor_WithLowerCaseDxCode_CorrectlyParsesArgument()
        {
            var dxCode = "dx666";
            var sut = new DispositionCode(dxCode);
            Assert.AreEqual("Dx666", sut.Value);
        }

        [Test]
        public void Ctor_WithCodeLackingPrefix_CorrectlyPrependsPrefix()
        {
            var dxCode = "666";
            var sut = new DispositionCode(dxCode);
            Assert.AreEqual("Dx666", sut.Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_WithNonParsableDxCode_ThrowsArgumentException()
        {
            var sut = new DispositionCode("SomeNonsense");
        }
    }
}

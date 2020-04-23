using NHS111.Models.Mappers.WebMappings;
using NUnit.Framework;
using System;
using System.Configuration;
namespace NHS111.Models.Test
{
    [TestFixture]
    public class DispositionResolverTests
    {
        private TestDispositionResolver _dispositionResolver;

        [SetUp]
        public void DispositionResolverTestsSetup()
        {
            _dispositionResolver = new TestDispositionResolver();
        }

        [Test]
        public void TextDxCode_Converted_correctly()
        {
            ConfigurationManager.AppSettings["ValidationDxRemap"] = "";
            var dxCode = "Dx02";
            var result = _dispositionResolver.TestResolveCore(dxCode);

            Assert.AreEqual(1002, result);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Invalid_DxCode_thows_FormatException()
        {
            ConfigurationManager.AppSettings["ValidationDxRemap"] = "";
            var dxCode = "InvalidCode";
            _dispositionResolver.TestResolveCore(dxCode);
        }
    }

    public class TestDispositionResolver : FromOutcomeViewModelToDosViewModel.DispositionResolver
    {
        public int TestResolveCore(string source)
        {
            return this.ResolveCore(source);
        }
    }
}

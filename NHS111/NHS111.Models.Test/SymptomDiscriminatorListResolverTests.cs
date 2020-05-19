using NHS111.Models.Mappers.WebMappings;
using NUnit.Framework;
using System;
using System.Linq;

namespace NHS111.Models.Test
{
    [TestFixture]
    public class SymptomDiscriminatorListResolverTests
    {
        private TestSymptomDiscriminatorListResolver _testSymptomDiscriminatorListResolver;
        [SetUp]
        public void SymptomDiscriminatorListResolverTestsSetup()
        {
            _testSymptomDiscriminatorListResolver = new TestSymptomDiscriminatorListResolver();
        }

        [Test]
        public void Valid_SDCode_Converted_correctly()
        {
            var sdString = "4000";
            var result = _testSymptomDiscriminatorListResolver.ResolveCore(sdString);

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(4000, result.First());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Empty_SDCode_ThowsExcpetion()
        {
            var sdString = "";
            _testSymptomDiscriminatorListResolver.ResolveCore(sdString);
        }

        [Test]
        public void Null_SDCode_Converted_toEmptyArray()
        {
            string sdString = null;
            var result = _testSymptomDiscriminatorListResolver.ResolveCore(sdString);

            Assert.AreEqual(result.Count(), 0);
        }
    }

    public class TestSymptomDiscriminatorListResolver : FromOutcomeViewModelToDosViewModel.SymptomDiscriminatorListResolver
    {
        public int[] ResolveCore(string source)
        {
            return base.ResolveCore(source);
        }
    }
}

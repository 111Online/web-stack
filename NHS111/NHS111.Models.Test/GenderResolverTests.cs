using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Web.Presentation.Models;
using NUnit.Framework;
using NHS111.Models.Models.Domain;
using System;

namespace NHS111.Models.Test
{
    [TestFixture]
    public class GenderResolverTests
    {
        private TestGenderResolver _genderResolver;

        [SetUp]
        public void DispositionResolverTestsSetup()
        {
            _genderResolver = new TestGenderResolver();
        }

        [Test]
        public void TextGenderString_Converted_correctly()
        {
            var genderString = "Female";
            var result = _genderResolver.TestResolveCore(genderString);

            Assert.AreEqual("F", result);
        }

        [Test]
        public void InvalidGenderString_Converted_correctly()
        {
            var genderString = "NotaGender";
            var result = _genderResolver.TestResolveCore(genderString);

            Assert.AreEqual("I", result);
        }

    }

    public class TestGenderResolver : FromOutcomeViewModelToDosViewModel.GenderResolver
    {
        public string TestResolveCore(string source)
        {
            return this.ResolveCore(source);
        }
    }
}
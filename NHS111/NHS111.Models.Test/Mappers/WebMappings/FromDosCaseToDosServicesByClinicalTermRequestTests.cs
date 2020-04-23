using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.DosRequests;
using NUnit.Framework;
using System;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture]
    public class FromDosCaseToDosServicesByClinicalTermRequestTests
    {
        private TestGenderResolver _genderResolver;
        private TestAgeResolver _ageResolver;
        private TestDispositionResolver _dispositionResolver;

        [SetUp]
        public void InitializeFromDosCaseToDosServicesByClinicalTermRequestTests()
        {
            _genderResolver = new TestGenderResolver();
            _ageResolver = new TestAgeResolver();
            _dispositionResolver = new TestDispositionResolver();
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.FromDosCaseToDosServicesByClinicalTermRequest>());
        }

        [Test]
        public void FromDosCaseToDosServicesByClinicalTermRequest_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void FromDosCaseToDosServicesByClinicalTermRequest_Test()
        {
            var dosCase = new DosCase()
            {
                CaseId = "xyz",
                PostCode = "xy1 z23",
                SearchDistance = 10,
                Age = "5",
                Gender = "F",
                Disposition = 10013,
                SymptomGroup = 1003,
                SymptomDiscriminator = 2010,
                NumberPerType = 10
            };

            var result = Mapper.Map<DosServicesByClinicalTermRequest>(dosCase);
            AssertValidModel(result);
        }

        private void AssertValidModel(DosServicesByClinicalTermRequest result)
        {
            Assert.AreEqual("xyz", result.CaseId);
            Assert.AreEqual("xy1 z23", result.Postcode);
            Assert.AreEqual("10", result.SearchDistance);
            Assert.AreEqual("0", result.GpPracticeId);
            Assert.AreEqual("2", result.Age);
            Assert.AreEqual("F", result.Gender);
            Assert.AreEqual("Dx013", result.Disposition);
            Assert.AreEqual("1003=2010", result.SymptomGroupDiscriminatorCombos);
            Assert.AreEqual("10", result.NumberPerType);
        }

        [Test]
        public void TextGenderEnum_Converted_correctly()
        {
            var resolvedFemale = _genderResolver.TestResolveCore(GenderEnum.Female);
            var resolvedMale = _genderResolver.TestResolveCore(GenderEnum.Male);
            var resolvedIndeterminate = _genderResolver.TestResolveCore(GenderEnum.Indeterminate);

            Assert.AreEqual("F", resolvedFemale);
            Assert.AreEqual("M", resolvedMale);
            Assert.AreEqual("I", resolvedIndeterminate);
        }

        [Test]
        public void TestAge_Converted_correctly()
        {
            var resolvedAdult = _ageResolver.TestResolveCore("16");
            var resolvedChild = _ageResolver.TestResolveCore("5");
            var resolvedToddler = _ageResolver.TestResolveCore("2");
            var resolvedOlder = _ageResolver.TestResolveCore("68");
            var resolvedNeonate = _ageResolver.TestResolveCore("-5");

            Assert.AreEqual(1, resolvedAdult);
            Assert.AreEqual(2, resolvedChild);
            Assert.AreEqual(3, resolvedToddler);
            Assert.AreEqual(4, resolvedNeonate);
            Assert.AreEqual(8, resolvedOlder);
        }

        [Test]
        public void TestInvalidAge_Converted_correctly()
        {
            var resolvedInvalidAge = _ageResolver.TestResolveCore("xyz");

            Assert.AreEqual(1, resolvedInvalidAge);
        }

        [Test]
        public void TestDisposition_Converted_correctly()
        {
            var resolvedDisposition = _dispositionResolver.TestResolveCore(1013);

            Assert.AreEqual("Dx13", resolvedDisposition);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestDisposition_Not_Starting_With_Ten_Throws_Correct_Error()
        {
            var resolvedDisposition = _dispositionResolver.TestResolveCore(1245);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestDisposition_Less_Than_Ten_Throws_Correct_Error()
        {
            var resolvedDisposition = _dispositionResolver.TestResolveCore(5);
        }

        public class TestGenderResolver : FromDosCaseToDosServicesByClinicalTermRequest.GenderResolver
        {
            public string TestResolveCore(GenderEnum source)
            {
                return this.ResolveCore(source);
            }
        }

        public class TestAgeResolver : FromDosCaseToDosServicesByClinicalTermRequest.AgeResolver
        {
            public int TestResolveCore(string source)
            {
                /*
                1= Adult (16+)
                2= Child (5-15)
                3= Toddler (1-4)
                4= Neonate and Infant (0)
                8= Older people (65+)
                */
                return this.ResolveCore(source);
            }
        }

        public class TestDispositionResolver : FromDosCaseToDosServicesByClinicalTermRequest.DispositionResolver
        {
            public string TestResolveCore(int source)
            {
                return this.ResolveCore(source);
            }
        }
    }
}
using System;
using System.Linq;
using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Models;
using NUnit.Framework;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Test
{
    [TestFixture]
    public class FromOutcomeViewModelToDosCaseTests
    {
        [SetUp]
        public void InitilaiseConverters()
        {
            AppDomain.CurrentDomain.GetAssemblies();
            Mapper.AddProfile(new FromOutcomeViewModelToDosCase());
        }

        [Test]
        public void FromOutcomeViewModelToDosCase_ConfiguredCorrectly()
        {
            Mapper.AssertConfigurationIsValid("FromOutcomeViewModelToDosCase");
        }

        [Test]
        public void Map_OutcomeViewModel_To_DosCase()
        {
            var viewModel = new OutcomeViewModel()
            {
                UserInfo = new UserInfo()
                {
                    Age = 22,
                    HomeAddress = new AddressInfo()
                    {
                        AddressLine1 = "1 Test lane",
                        City = "TestTown"
                    },
                    Gender = "Female"
                },
                Id = "Dx02",
                SymptomGroup = "1056",
                SymptomDiscriminator = "2222"
            };

            var result = Mapper.Map<OutcomeViewModel, DosCase>(viewModel);

            Assert.AreEqual("22", result.Age);
            Assert.AreEqual(1056, result.SymptomGroup);
            Assert.AreEqual(1002, result.Disposition);
            Assert.AreEqual(GenderEnum.Female, result.Gender);
            Assert.IsTrue(result.SymptomDiscriminatorList.Contains(2222));
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Map_OucomeViewModel_To_DosCase_Invalid_DxCode()
        {
            var viewModel = new OutcomeViewModel()
            {
                UserInfo = new UserInfo()
                {
                    Age = 22,
                    HomeAddress = new AddressInfo()
                    {
                        AddressLine1 = "1 Test lane",
                        City = "TestTown"
                    },
                    Gender = "Female"
                },
                Id = "1332",
                SymptomGroup = "1056",
                SymptomDiscriminator = "2222"
            };

            var result = Mapper.Map<OutcomeViewModel, DosCase>(viewModel);
        }
    }
}

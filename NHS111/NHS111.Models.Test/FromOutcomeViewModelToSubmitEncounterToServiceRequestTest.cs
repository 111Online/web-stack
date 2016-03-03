using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Models;
using NUnit.Framework;

namespace NHS111.Models.Test
{
    [TestFixture]
    public class FromOutcomeViewModelToSubmitEncounterToServiceRequestTest
    {
        [SetUp]
        public void InitilaiseConverters()
        {
            AppDomain.CurrentDomain.GetAssemblies();
            Mapper.AddProfile(new FromOutcomeViewModelToSubmitEncounterToServiceRequest());
        }

        [Test]
        public void FromOutcomeViewModelToDosCase_ConfiguredCorrectly()
        {
            Mapper.AssertConfigurationIsValid("FromOutcomeViewModelToSubmitEncounterToServiceRequest");
        }

        [Test]
        public void Map_OutcomeViewModel_To_SubmitEncounterToServiceRequest()
        {
            var viewModel = new OutcomeViewModel()
            {
                UserInfo = new UserInfo()
                {
                    FirstName = "Billy Bob",
                    LastName = "Thornton",
                    Age = 44,
                    HomeAddress = new AddressInfo()
                    {
                        HouseNumber = "101",
                        AddressLine1 = "The Road",
                        AddressLine2 = "The Village",
                        City = "The Town",
                        County = "The County",
                        PostCode = "PO5 6GH"
                    },
                    CurrentAddress = new AddressInfo()
                    {
                        HouseNumber = "101",
                        AddressLine1 = "The Road",
                        AddressLine2 = "The Village",
                        City = "The Town",
                        County = "The County",
                        PostCode = "PO5 6GH"
                    },
                    Gender = "Male",
                    Day = 30,
                    Month = 5,
                    Year = 1972,
                    TelephoneNumber = "02070 033002"
                },
                Id = "Dx02",
                
                SymptomGroup = "1056",
                SymptomDiscriminator = "2222",
                SelectedServiceId = "1345754835",
                CheckCapacitySummaryResultList = new []
                {
                    new CheckCapacitySummaryResult()
                    {
                        IdField = 1345754835,
                        NameField = "Test Service",
                        OdsCodeField = "1345754835",
                        AddressField = "150 The Road",
                        PostcodeField = "IG7 3GJ"
                    }
                },
                
            };

            var result = Mapper.Map<OutcomeViewModel, DosCase>(viewModel);

            Assert.AreEqual("22", result.Age);
            Assert.AreEqual(1056, result.SymptomGroup);
            Assert.AreEqual(1002, result.Disposition);
            Assert.AreEqual(GenderType.F, result.Gender);
            Assert.IsTrue(result.SymptomDiscriminatorList.Contains(2222));
        }
    }
}

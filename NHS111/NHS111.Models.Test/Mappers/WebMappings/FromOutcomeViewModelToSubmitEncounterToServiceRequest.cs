using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NUnit.Framework;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture]
    public class FromOutcomeViewModelToSubmitEncounterToServiceRequest
    {
        [TestFixtureSetUp]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.FromOutcomeViewModelToSubmitEncounterToServiceRequest>());
        }

        [Test]
        public void FromOutcomeViewModelToPatientDetailsConverter_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void FromOutcomeViewModelToPatientDetailsConverter_Informant_null_test()
        {
            var outcome = new OutcomeViewModel()
            {
                UserInfo = new UserInfo()
                {
                    FirstName = "Test",
                    LastName = "User",
                    Demography = new AgeGenderViewModel()
                    {
                        Age = 35,
                        Gender = "Male"
                    },
                    TelephoneNumber = "111",
                },
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult()
                {
                    Success = new SuccessObject<ServiceViewModel>()
                    {
                        Services = new List<ServiceViewModel>() { new ServiceViewModel() { Id = 1, PostCode = "So30 2un" } }
                    }
                },
                SelectedServiceId = "1",
                AddressInfoViewModel = new PersonalDetailsAddressViewModel()
                {
                    AddressLine1 = "address 1",
                    AddressLine2 = "address 2",
                    City = "Testity",
                    County = "Tesux",
                    HouseNumber = "1",
                    Postcode = "111 111",
                },

            };

            var result = Mapper.Map<OutcomeViewModel, PatientDetails>(outcome);
            Assert.AreEqual("111", result.Informant.TelephoneNumber);
            Assert.AreEqual(NHS111.Models.Models.Web.ITK.InformantType.Self, result.Informant.Type);
        }

        [Test]
        public void FromOutcomeViewModelToPatientDetailsConverter_Informant_false_test()
        {
            var outcome = new OutcomeViewModel()
            {
                UserInfo = new UserInfo()
                {
                    FirstName = "Test",
                    LastName = "User",
                    Demography = new AgeGenderViewModel()
                    {
                        Age = 35,
                        Gender = "Male"
                    },
                    TelephoneNumber = "111",
                },
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult()
                {
                    Success = new SuccessObject<ServiceViewModel>()
                    {
                        Services = new List<ServiceViewModel>() { new ServiceViewModel() { Id = 1, PostCode = "So30 2un" } }
                    }
                },
                SelectedServiceId = "1",
                AddressInfoViewModel = new PersonalDetailsAddressViewModel()
                {
                    AddressLine1 = "address 1",
                    AddressLine2 = "address 2",
                    City = "Testity",
                    County = "Tesux",
                    HouseNumber = "1",
                    Postcode = "111 111",
                },
                Informant = new InformantViewModel()
                {
                    IsInformantForPatient = false
                }
            };

            var result = Mapper.Map<OutcomeViewModel, PatientDetails>(outcome);
            Assert.AreEqual("111", result.Informant.TelephoneNumber);
            Assert.AreEqual(NHS111.Models.Models.Web.ITK.InformantType.Self, result.Informant.Type);
        }

        [Test]
        public void FromOutcomeViewModelToPatientDetailsConverter_Informant_true_test()
        {
            var outcome = new OutcomeViewModel()
            {
                UserInfo = new UserInfo()
                {
                    FirstName = "Test",
                    LastName = "User",
                    Demography = new AgeGenderViewModel()
                    {
                        Age = 35,
                        Gender = "Male"
                    },
                    TelephoneNumber = "111",
                },
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult()
                {
                    Success = new SuccessObject<ServiceViewModel>()
                    {
                        Services = new List<ServiceViewModel>() { new ServiceViewModel() { Id = 1, PostCode = "So30 2un" } }
                    }
                },
                SelectedServiceId = "1",
                AddressInfoViewModel = new PersonalDetailsAddressViewModel()
                {
                    AddressLine1 = "address 1",
                    AddressLine2 = "address 2",
                    City = "Testity",
                    County = "Tesux",
                    HouseNumber = "1",
                    Postcode = "111 111",
                },
                Informant = new InformantViewModel()
                {
                    Forename = "Informer",
                    Surname = "bormer",
                    IsInformantForPatient = true
                }
            };

            var result = Mapper.Map<OutcomeViewModel, PatientDetails>(outcome);
            Assert.AreEqual("Informer", result.Informant.Forename);
            Assert.AreEqual("bormer", result.Informant.Surname);
            Assert.AreEqual("111", result.Informant.TelephoneNumber);
            Assert.AreEqual(NHS111.Models.Models.Web.ITK.InformantType.NotSpecified, result.Informant.Type);
        }
    }
}

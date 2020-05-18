using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NUnit.Framework;
using InformantType = NHS111.Models.Models.Web.InformantType;

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
        public void FromPersonalDetailViewModelToPatientDetailsConverter_Informant_null_test()
        {
            var outcome = new PersonalDetailViewModel()
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
                AddressInformation = new LocationInfoViewModel()
                {
                    PatientCurrentAddress = new CurrentAddressViewModel()
                    {
                        AddressLine1 = "address 1",
                        AddressLine2 = "address 2",
                        City = "Testity",
                        County = "Tesux",
                        HouseNumber = "1",
                        Postcode = "111 111",
                    }
                },

            };

            var result = Mapper.Map<PersonalDetailViewModel, PatientDetails>(outcome);
            Assert.AreEqual("111", result.Informant.TelephoneNumber);
            Assert.AreEqual(NHS111.Models.Models.Web.ITK.InformantType.Self, result.Informant.Type);
        }

        [Test]
        public void FromPersonalDetailViewModelToPatientDetailsConverter_Informant_false_test()
        {
            var outcome = new PersonalDetailViewModel()
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
                AddressInformation = new LocationInfoViewModel() { 
                    PatientCurrentAddress = new CurrentAddressViewModel()
                {
                    AddressLine1 = "address 1",
                    AddressLine2 = "address 2",
                    City = "Testity",
                    County = "Tesux",
                    HouseNumber = "1",
                    Postcode = "111 111",
                }},
                Informant = new InformantViewModel()
                
            };

            var result = Mapper.Map<OutcomeViewModel, PatientDetails>(outcome);
            Assert.AreEqual("111", result.Informant.TelephoneNumber);
            Assert.AreEqual(NHS111.Models.Models.Web.ITK.InformantType.Self, result.Informant.Type);
        }

        [Test]
        public void FromPersonalDetailViewModelToPatientDetailsConverter_Informant_true_test()
        {
            var outcome = new PersonalDetailViewModel()
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
                AddressInformation = new LocationInfoViewModel()
                {
                    PatientCurrentAddress = new CurrentAddressViewModel()
                    {
                    AddressLine1 = "address 1",
                    AddressLine2 = "address 2",
                    City = "Testity",
                    County = "Tesux",
                    HouseNumber = "1",
                    Postcode = "111 111",
                }},
                Informant = new InformantViewModel()
                {
                    Forename = "Informer",
                    Surname = "bormer",
                    InformantType = InformantType.ThirdParty
                   
                }
            };

            var result = Mapper.Map<OutcomeViewModel, PatientDetails>(outcome);
            Assert.AreEqual("Informer", result.Informant.Forename);
            Assert.AreEqual("bormer", result.Informant.Surname);
            Assert.AreEqual("111", result.Informant.TelephoneNumber);
            Assert.AreEqual(NHS111.Models.Models.Web.ITK.InformantType.NotSpecified, result.Informant.Type);
        }

        [Test]
        public void FromOutcomeViewModelToCaseStepsConverter_test()
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
                Journey = new Journey
                {
                    Steps = new List<JourneyStep>
                    {
                        new JourneyStep
                        {
                            QuestionId = "Tx12345",
                            Answer = new Answer { Order = 0 }
                        },
                        new JourneyStep
                        {
                            QuestionId = "Tx678910",
                            Answer = new Answer { Order = 3 }
                        },
                        new JourneyStep
                        {
                            QuestionId = "Tx111111",
                            Answer = new Answer { Order = 2 }
                        },
                    }
                }
            };

            var result = Mapper.Map<OutcomeViewModel, CaseDetails>(outcome);
            Assert.AreEqual(3, result.CaseSteps.Count());
            Assert.AreEqual("Tx12345", result.CaseSteps.First().QuestionId);
            Assert.AreEqual(0, result.CaseSteps.First().AnswerOrder);
            Assert.AreEqual("Tx678910", result.CaseSteps.Skip(1).First().QuestionId);
            Assert.AreEqual(3, result.CaseSteps.Skip(1).First().AnswerOrder);
            Assert.AreEqual("Tx111111", result.CaseSteps.Skip(2).First().QuestionId);
            Assert.AreEqual(2, result.CaseSteps.Skip(2).First().AnswerOrder);
        }

        [Test]
        public void FromPersonalDetailViewModelToPatientDetailsConverter_AgeGroup_Adult_test()
        {
            var outcome = new PersonalDetailViewModel()
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
                AddressInformation = new LocationInfoViewModel()
                {
                    PatientCurrentAddress = new CurrentAddressViewModel()
                    {
                        AddressLine1 = "address 1",
                        AddressLine2 = "address 2",
                        City = "Testity",
                        County = "Tesux",
                        HouseNumber = "1",
                        Postcode = "111 111",
                    }
                },
                Informant = new InformantViewModel()
                {
                    Forename = "Informer",
                    Surname = "bormer",
                    InformantType = InformantType.ThirdParty
                }
            };

            var result = Mapper.Map<PersonalDetailViewModel, PatientDetails>(outcome);
            Assert.AreEqual("Adult", result.AgeGroup);
        }

        [Test]
        public void FromPersonalDetailViewModelToPatientDetailsConverter_AgeGroup_Child_test()
        {
            var outcome = new PersonalDetailViewModel()
            {
                UserInfo = new UserInfo()
                {
                    FirstName = "Test",
                    LastName = "User",
                    Demography = new AgeGenderViewModel()
                    {
                        Age = 15,
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
                AddressInformation = new LocationInfoViewModel()
                {
                    PatientCurrentAddress = new CurrentAddressViewModel()
                    {
                        AddressLine1 = "address 1",
                        AddressLine2 = "address 2",
                        City = "Testity",
                        County = "Tesux",
                        HouseNumber = "1",
                        Postcode = "111 111",
                    }
                },
                Informant = new InformantViewModel()
                {
                    Forename = "Informer",
                    Surname = "bormer",
                   InformantType = InformantType.ThirdParty
                }
            };

            var result = Mapper.Map<PersonalDetailViewModel, PatientDetails>(outcome);
            Assert.AreEqual("Child", result.AgeGroup);
        }

        [Test]
        public void FromOutcomeViewModelToCaseDetailsConverter_Condition_test()
        {
            var outcome = new PersonalDetailViewModel()
            {
                PathwayTitle = "Skin Problems",
                PathwayId = "PW123MaleChild",
                PathwayTraumaType = "Trauma",
                Journey = new Journey
                {
                    Steps = new List<JourneyStep>
                    {
                        new JourneyStep
                        {
                            QuestionId = "Tx12345",
                            Answer = new Answer { Order = 0 }
                        },
                    }
                }
            };

            var result = Mapper.Map<OutcomeViewModel, CaseDetails>(outcome);
            Assert.AreEqual("Skin Problems", result.StartingPathwayTitle);
            Assert.AreEqual("PW123MaleChild", result.StartingPathwayId);
            Assert.AreEqual("Trauma", result.StartingPathwayType);
        }

        [Test]
        public void FromOutcomeViewModelToCaseDetailsConverter_Set_Variables_test()
        {
            var outcome = new PersonalDetailViewModel()
            {
                Journey = new Journey
                {
                    Steps = new List<JourneyStep>
                    {
                        new JourneyStep
                        {
                            QuestionId = "Tx12345",
                            Answer = new Answer { Order = 0 },
                            State = "{\"PATIENT_AGE\":\"42\",\"PATIENT_GENDER\":\"\\\"M\\\"\",\"PATIENT_PARTY\":\"1\",\"PATIENT_AGEGROUP\":\"Adult\",\"SYSTEM_MERS\":\"mers\",\"SYSTEM_ONLINE\":\"online\"}"
                        },
                    }
                }
            };

            var result = Mapper.Map<OutcomeViewModel, CaseDetails>(outcome);
            Assert.AreEqual(6, result.SetVariables.Count);
            Assert.IsTrue(result.SetVariables.ContainsKey("PATIENT_AGEGROUP"));
            Assert.AreEqual("Adult", result.SetVariables["PATIENT_AGEGROUP"]);
        }
    }
}

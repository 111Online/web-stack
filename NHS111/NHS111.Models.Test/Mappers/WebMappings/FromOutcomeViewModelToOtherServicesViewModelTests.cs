using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture]
    public class FromOutcomeViewModelToOtherServicesViewModelTests
    {
        private OutcomeViewModel _minimumViableOutcomeViewModel;

        [TestFixtureSetUp]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<FromOutcomeViewModelToOtherServicesViewModel>());
            _minimumViableOutcomeViewModel = GenerateMinimumObject();
        }

        private OutcomeViewModel GenerateMinimumObject()
        {
            SymptomDiscriminator symptomDiscriminator = new SymptomDiscriminator();
            symptomDiscriminator.Description = "Desc";
            symptomDiscriminator.Id = 30;
            symptomDiscriminator.ReasoningText = "Reasoning";

            return new OutcomeViewModel()
            {
                Id = "Dx20",
                SymptomDiscriminator = symptomDiscriminator,
                SymptomDiscriminatorCode = "123",

                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
                        Gender = "M"
                    },
                    CurrentAddress = new FindServicesAddressViewModel
                    {
                        Postcode = "PO57CD"
                    }
                },
                Journey = new Journey
                {
                    Steps = new List<JourneyStep>()
                },
                RecommendedService = new RecommendedServiceViewModel()
                {
                    PublicName = "Test recommendation",
                    ReasonText = "There is a reason for this"
                }
            };
        }

        [Test]
        public void FromQuestionToJourneyViewModelConverter_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void FromOutcomeViewModelToOtherServicesViewModel_WithReasonText_MapsReasonText()
        {
            var result = Mapper.Map<OtherServicesViewModel>(_minimumViableOutcomeViewModel);
            Assert.AreEqual("There is a reason for this", result.RecommendedService.ReasonText);
        }
    }
}

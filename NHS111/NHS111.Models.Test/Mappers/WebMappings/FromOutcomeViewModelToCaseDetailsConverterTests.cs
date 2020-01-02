
namespace NHS111.Models.Test.Mappers.WebMappings
{
    using System.Collections.Generic;
    using AutoMapper;
    using System.Configuration;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NUnit.Framework;

    [TestFixture]
    public class FromOutcomeViewModelToCaseDetailsConverterTests
    {   
        private OutcomeViewModel _minimumViableOutcomeViewModel;

        [TestFixtureSetUp]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<FromOutcomeVIewModelToITKDispatchRequest>());
            
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
                Journey = new Journey {
                    Steps = new List<JourneyStep>()
                }
            };
        }

        [Test]
        public void FromOutcomeViewModelToDosViewModelConverter_WithCat3DxCode_RemapsToDx333() {
            _minimumViableOutcomeViewModel.Id = "Dx012";
            _minimumViableOutcomeViewModel.HasAcceptedCallbackOffer = true;
            ConfigurationManager.AppSettings["ValidationDxRemap"] = "Dx012:Dx333";
            var result = Mapper.Map<CaseDetails>(_minimumViableOutcomeViewModel);

            Assert.AreEqual("Dx333", result.DispositionCode);
        }

        [Test]
        public void FromOutcomeViewModelToDosViewModelConverter_WithEDDxCode_RemapsToDx334() {
            _minimumViableOutcomeViewModel.Id = "Dx02";
            _minimumViableOutcomeViewModel.HasAcceptedCallbackOffer = true;

            ConfigurationManager.AppSettings["ValidationDxRemap"] = "Dx02:Dx334";
            var result = Mapper.Map<CaseDetails>(_minimumViableOutcomeViewModel);

            Assert.AreEqual("Dx334", result.DispositionCode);
        }

    }
}

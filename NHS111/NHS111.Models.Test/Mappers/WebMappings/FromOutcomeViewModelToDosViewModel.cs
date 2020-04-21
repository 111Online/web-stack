using AutoMapper;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NUnit.Framework;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture]
    public class FromOutcomeViewModelToDosViewModel
    {
        private OutcomeViewModel _minimumViableOutcomeViewModel;

        [TestFixtureSetUp]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.FromOutcomeViewModelToDosViewModel>());

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
                }
            };
        }

        [Test]
        public void FromOutcomeViewModelToDosViewModelConverter_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void FromOutcomeViewModelToDosViewModelConverter_DXCodeTwoDigits()
        {
            _minimumViableOutcomeViewModel.Id = "Dx20";

            var result = Mapper.Map<OutcomeViewModel, DosViewModel>(_minimumViableOutcomeViewModel);
            Assert.AreEqual(1020, result.Disposition);
        }

        [Test]
        public void FromOutcomeViewModelToDosViewModelConverter_DXCodeThreeDigits()
        {
            _minimumViableOutcomeViewModel.Id = "Dx329";

            var result = Mapper.Map<OutcomeViewModel, DosViewModel>(_minimumViableOutcomeViewModel);
            Assert.AreEqual(11329, result.Disposition);
        }

        [Test]
        public void FromOutcomeViewModelToDosViewModelConverter_DXCodeThreeDigitsStartingWithOne()
        {
            _minimumViableOutcomeViewModel.Id = "Dx118";

            var result = Mapper.Map<OutcomeViewModel, DosViewModel>(_minimumViableOutcomeViewModel);
            Assert.AreEqual(1118, result.Disposition);
        }
    }
}

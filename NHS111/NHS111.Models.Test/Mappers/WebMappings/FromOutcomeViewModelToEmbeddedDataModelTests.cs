using System;
using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Business.MicroSurvey;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture]
    public class FromOutcomeViewModelToEmbeddedDataModelTests
    {
        [TestFixtureSetUp]
        public void InitialiseModelMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<FromOutcomeViewModelToEmbeddedDataModel>();
            });
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_JourneyId()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.JourneyId = Guid.Parse("ddb6194f-c4ff-4fc3-b74b-78cf66ee304c"), ed => ed.JourneyId, "ddb6194f-c4ff-4fc3-b74b-78cf66ee304c");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_DispositionCode()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.SurveyLink.DispositionCode = "Dx123", ed => ed.DxCode, "Dx123");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_DispositionDate()
        {
            var dateTime = new DateTime(2002, 11, 2, 3, 15, 33);

            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.SurveyLink.DispositionDateTime = dateTime, ed => ed.DispositionTime, dateTime.TimeOfDay.ToString());
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_Source()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.Source = "TestSource123", ed => ed.Ccg, "TestSource123");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_ServiceOptions()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.SurveyLink.ServiceOptions = "TestServiceOptions", ed => ed.ServicesOffered, "TestServiceOptions");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_ServiceCount()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.SurveyLink.ServiceCount = 17, ed => ed.ServiceCount, "17");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_OnlineDoSServiceType()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.RecommendedService = new RecommendedServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback }, ed => ed.RecommendedServiceDosType, OnlineDOSServiceType.Callback.ToString());
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_RecommendedService_Id()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.RecommendedService = new RecommendedServiceViewModel() { Id = 14 }, ed => ed.RecommendedServiceId, "14");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_RecommendedService_Name()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.RecommendedService = new RecommendedServiceViewModel() { Name = "TestName123" }, ed => ed.ResommendedServiceName, "TestName123");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_RecommendedService_Alias()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.RecommendedService = new RecommendedServiceViewModel(){ PublicName = "TestAlias123"}, ed => ed.RecommendedServiceAlias, "TestAlias123");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_RecommendedService_Distance()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.RecommendedService = new RecommendedServiceViewModel() { Distance = "TestDistance123" }, ed => ed.RecommendedServiceDistance, "TestDistance123");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_RecommendedService_SymptomDiscriminatorCode()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.SymptomDiscriminatorCode = "TestSdCode123", ed => ed.SdCode, "TestSdCode123");
        }

        [Test]
        public void FromOutcomeViewModelToEmbeddedDataModel_Mapper_Maps_Correct_For_RecommendedService_SymptomDiscriminatorDescription()
        {
            FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(o => o.SymptomDiscriminator = new SymptomDiscriminator(){ Description = "TestSdDescription123" }, ed => ed.SdDescription, "TestSdDescription123");
        }

        private void FromOutcomeViewModelToEmbeddDataModel_Mapper_TestHelper(Action<OutcomeViewModel> setOutcomeModelProp, Func<EmbeddedData, string> getEmbededDataProp, string expected)
        {
            var outcomeViewModel = new OutcomeViewModel();

            setOutcomeModelProp(outcomeViewModel);

            var embeddedData = Mapper.Map<EmbeddedData>(outcomeViewModel);

            var actual = getEmbededDataProp(embeddedData);

            Assert.AreEqual(expected, actual);
        }
    }
}

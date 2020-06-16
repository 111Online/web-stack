using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    [ScreenShotComparison]
    public class RecommendedServiceTests : BaseTests
    {
        [Test]
        public void PreOutcomePageDisplayed()
        {
            var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "L12SA");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var preOutcomePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1);

            preOutcomePage.VerifyHeader();
            preOutcomePage.VerifySubHeader();
            preOutcomePage.VerifyShowServicesButton();
            preOutcomePage.CompareAndVerify("1");
        }

        [Test]
        public void ReferRingAndGoService()
        {
            var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "OX1 1DJ");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyServiceDetails();
            recommendedServicePage.VerifyOpeningTimes();
            recommendedServicePage.VerifyDistance();
            recommendedServicePage.VerifyActionLink("Refer me to this service");
            recommendedServicePage.CompareAndVerify("1");
        }

        [Test]
        public void ReferOnlyService()
        {
            var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "OL9 6BW");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyServiceDetails();
            recommendedServicePage.VerifyActionLink("Use this service");
            recommendedServicePage.VerifyOpeningTimes();
            recommendedServicePage.CompareAndVerify("1");
        }

        [Test]
        public void PlaceToVisitService()
        {
            var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "L18BQ");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyServiceDetails();
            recommendedServicePage.VerifyOpeningTimes();
            recommendedServicePage.VerifyDistance();
            recommendedServicePage.VerifyNoActionLink();
            recommendedServicePage.CompareAndVerify("1");
        }

        [Test]
        public void PlaceToPhoneService()
        {
            var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "L18BN");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyServiceDetails();
            recommendedServicePage.VerifyNoActionLink();
            recommendedServicePage.CompareAndVerify("1");
        }

        [Test]
        public void OtherServicesMoreThanOneService()
        {
            var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "BB17JJ");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyOtherServices();
            recommendedServicePage.CompareAndVerify("1");
        }

        //[Test]
        //public void OtherServicesNotShown()
        //{
        //    var questionPage = TestScenarios.LaunchRecommendedServiceScenario(Driver, "Emergency Prescription 111 online", TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "MK181EG");
        //
        //    questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
        //    var recommendedServicePage = questionPage
        //        .Answer(2)
        //        .Answer<PreOutcomePage>(1)
        //        .ClickShowServices();
        //
        //    recommendedServicePage.VerifyNoOtherServices();
        //    recommendedServicePage.CompareAndVerify("1");
        //}
    }
}

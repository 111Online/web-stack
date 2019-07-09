using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class SurveyTests : BaseTests
    {

        /*
         * These tests ensure the model is correct for passing through to the survey.
         * They ensure we don't have issues such as the Pathways Title being passed
         * through instead of the Digital Title.
         */

        [Test]
        public void DigitalTitleThroughSearch()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Arm Injury, Penetrating",
                TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult);

            questionPage.VerifyHiddenField("PathwayTitle", "Arm Injury, Penetrating");
            questionPage.VerifyHiddenField("DigitalTitle", "Arm or shoulder injury with a cut or wound");
        }

        [Test]
        public void DigitalTitleThroughCategoryAllTopics()
        {
            var categoryPage = TestScenerios.LaunchCategoryScenerio(Driver, TestScenerioSex.Female, 64);
            var questionPage = TestScenarioPart.Question(categoryPage.SelectPathway("Arm or shoulder injury with a cut or wound"));

            questionPage.VerifyHiddenField("PathwayTitle", "Arm Injury, Penetrating");
            questionPage.VerifyHiddenField("DigitalTitle", "Arm or shoulder injury with a cut or wound");
        }

        [Test]
        public void DigitalTitleThroughDeeplink()
        {
            var questionPage = TestScenerios.LaunchDeeplinkScenerio(Driver, TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "L1 2SA");

            questionPage.VerifyHiddenField("PathwayTitle", "Emergency Prescription 111 online");
            questionPage.VerifyHiddenField("DigitalTitle", "Emergency prescription");
        }
    }
}

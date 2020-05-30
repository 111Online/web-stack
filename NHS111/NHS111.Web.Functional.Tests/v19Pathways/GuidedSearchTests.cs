using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests.v19Pathways
{
    public class GuidedSearchTests : BaseTests
    {
        private QuestionPage LaunchViaCovidLink(string sex, int age)
        {
            var homepage = TestScenarioPart.HomePage(Driver);
            var covidHomePage = homepage.ClickCovidLink();
            //covidHomePage.VerifyCovidPathway();
            var locationPage = covidHomePage.ClickOnStartNow();
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Question(demographicsPage, sex, age);
        }

        [Test]
        public void NavigateToCoughPW975FemaleAdult()
        {
            var guidedSelectionPage = LaunchViaCovidLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            //Not question page, guidedselectionpage
            var weirdQuestionPage = guidedSelectionPage.guidedSelection("Cough", true);

            //Verify the covid message appears
            //Confirm PathwayNo = PW854
            weirdQuestionPage.VerifyHiddenField("PathwayNo", "PW975");

            //click Iunderstand



            //Now go through questions

            //var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
            //    .Answer(2) // no
            //    .Answer(1) // yes
            //    .Answer(3) // no
            //    .Answer(3) // no
            //    .Answer(1) // Normal,warmorhot
            //    .Answer(3) // No - I feel well enough to do most of my usual daily activities 
            //    .Answer(2) // I'm not sure
            //    .Answer(2) // I'm not sure
            //    .Answer<OutcomePage>(2); // I'm notsure
            //outcomePage.VerifyHiddenField("Id", "Dx391");
        }
    }
}

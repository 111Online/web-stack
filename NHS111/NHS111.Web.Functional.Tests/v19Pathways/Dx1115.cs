using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.v19Pathways

{
    [TestFixture]
    class Dx1115 : BaseTests
    {
        private QuestionPage LaunchCovidWithLink(string sex, int age)
        {
            var homepage = TestScenarioPart.HomePage(Driver);
            var covidHomePage = homepage.ClickCovidLink();
            covidHomePage.VerifyCovidPathway();
            var locationPage = covidHomePage.ClickOnStartNow();
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Question(demographicsPage, sex, age);
        }

        [Test]
        public void NavigateToDispositionDx1115_6Hours()
        {
            var questionPage = LaunchCovidWithLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
            .Answer(2) // no - Cough
                    .Answer(1) // yes - fever
                    .Answer(3) // no - smell
                    .Answer(1) // Yes - Breathless
                    .Answer(2) // No - Unable to speak
                    .Answer(3) // Cool - skin feel
                    .Answer(3) // No - Pale
                    .Answer(3) // No - Breathing faster
                    .Answer(3) // No - I feel well enough to do most of my usual daily activities 
                    .Answer(2) // I'm not sure - More confused
                    .Answer(2) // I'm not sure - Serious Infection
                    .Answer<OutcomePage>(2); // I'm not sure - had a letter
            outcomePage.VerifyHiddenField("Id", "Dx1115");
        }

    }
}

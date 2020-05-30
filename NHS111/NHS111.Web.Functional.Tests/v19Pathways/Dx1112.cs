using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.v19Pathways
{
    [TestFixture]
    class Dx1112 : BaseTests
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
        public void NavigateToDispositionDx1112_BreathingBetter()
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
                    .Answer(1) // Yes - Breathing faster
                    .Answer<OutcomePage>(1); // Better - breathing in the last hour
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }

        [Test]
        public void NavigateToDispositionDx1112_BreathingSame()
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
                    .Answer(1) // Yes - Breathing faster
                    .Answer<OutcomePage>(3); // Same - breathing in the last hour
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }

        [Test]
        public void NavigateToDispositionDx1112_BreathingNotSure()
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
                    .Answer(1) // Yes - Breathing faster
                    .Answer<OutcomePage>(4); // Not sure - breathing in the last hour
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }
    }
}

using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.v19Pathways

{
    [TestFixture]
    class Dx1113 : BaseTests
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
        public void NavigateToDispositionDx1113()
        {
            var questionPage = LaunchCovidWithLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
                .Answer(2) // no
                .Answer(1) // yes
                .Answer(3) // no
                .Answer(3) // no
                .Answer(1) // Normal,warmorhot
                .Answer<OutcomePage>(1); // Yes, I stopped doing everything what i do
            outcomePage.VerifyHiddenField("Id", "Dx1113");
        }

        [Test]
        public void NavigateToDispositionDx1113Journey2()
        {
            var questionPage = LaunchCovidWithLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
                .Answer(2) // no
                .Answer(1) // yes
                .Answer(3) // no
                .Answer(3) // no
                .Answer(1) // Normal,warmorhot
                .Answer(3) // No - I feel well enough to do most of my usual daily activities 
                .Answer<OutcomePage>(1); // Yes
            outcomePage.VerifyHiddenField("Id", "Dx1113");
            outcomePage.VerifyHasButton(buttonName: "PersonalDetails", buttonValue: "Book a call");
        }
    }
}

using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.Covid
{
    [TestFixture]
    class Dx1116 : BaseTests
    {
        private QuestionPage LaunchWithCovidLink(string sex, int age, string guidedSelection)
        {
            var homepage = TestScenarioPart.HomePage(Driver);
            var covidHomePage = homepage.ClickCovidLink();
            var locationPage = covidHomePage.ClickOnStartNow();
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var guidedSelectionPage = TestScenarioPart.Question(demographicsPage, sex, age);
            var weirdQuestionPage = guidedSelectionPage.guidedSelection(guidedSelection);

            return weirdQuestionPage.AnswerWeirdQuestion();
        }

        [Test]
        public void NavigateToDispositionDx1116()
        {
            //0,0,2,2,0,2,2,2,2,2,2,2,2,2,0,2,2,3
            var questionPage = LaunchWithCovidLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - smell
                .Answer(3) // No - hurt head
                .Answer(3) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(3) // No - breathing harder
                .Answer(3) // No - so ill
                .Answer(3) // No - sharp pain
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer(3) // No - coughed blood
                .Answer(3) // No - confused
                .Answer(3) // No - doctor told you
                .Answer(3) // No - NHS letter
                .Answer(1) // Yes - diabetes
                .Answer(3) // No - blood sugar
                .Answer(3) // Not sure - either of these
                .Answer<OutcomePage>(4); // No - temperature
            outcomePage.VerifyHiddenField("Id", "Dx1116");
        }


    }
}

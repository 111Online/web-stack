using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.Covid
{
    [TestFixture]
    class Dx1112 : BaseTests
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
        public void NavigateToDispositionDx1112_LossOfTasteOrSmell()
        {
            var questionPage = LaunchWithCovidLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - loss of smell
                .Answer(3) // No - hurt head
                .Answer(3) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(3) // No - breathing harder
                .Answer(1) // Yes - so ill
                .Answer(3) // No - bruises
                .Answer(4) // No - meningitis
                .Answer(1) // Yes - confused
                .Answer(3) // No - sharp pain
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer<OutcomePage>(1); // Yes - coughed blood
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }

        [Test]
        public void NavigateToDispositionDx1112_LossOfTasteOrSmell_BreathingBetter()
        {
            var questionPage = LaunchWithCovidLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - loss of smell
                .Answer(3) // No - hurt head
                .Answer(3) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(1) // Yes - breathing harder
                .Answer(1) // Yes - so ill
                .Answer(3) // No - bruises
                .Answer(4) // No - meningitis
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer(3) // No - confused
                .Answer(3) // No - coughed blood
                .Answer(1) // Yes - this bad before
                .Answer(3) // No - been to hospital
                .Answer<OutcomePage>(1); // Better than before
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }

        [Test]
        public void NavigateToDispositionDx1112_LossOfTasteOrSmell_BreathingSame()
        {
            var questionPage = LaunchWithCovidLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - loss of smell
                .Answer(3) // No - hurt head
                .Answer(3) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(1) // Yes - breathing harder
                .Answer(1) // Yes - so ill
                .Answer(3) // No - bruises
                .Answer(4) // No - meningitis
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer(3) // No - confused
                .Answer(3) // No - coughed blood
                .Answer(1) // Yes - this bad before
                .Answer(3) // No - been to hospital
                .Answer<OutcomePage>(3); //Same as before
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }

        [Test]
        public void NavigateToDispositionDx1112_LossOfTasteOrSmell_BreathingNotSure()
        {
            var questionPage = LaunchWithCovidLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - loss of smell
                .Answer(3) // No - hurt head
                .Answer(3) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(1) // Yes - breathing harder
                .Answer(1) // Yes - so ill
                .Answer(3) // No - bruises
                .Answer(4) // No - meningitis
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer(3) // No - confused
                .Answer(3) // No - coughed blood
                .Answer(1) // Yes - this bad before
                .Answer(3) // No - been to hospital
                .Answer<OutcomePage>(4); //Breathing not sure
            outcomePage.VerifyHiddenField("Id", "Dx1112");
        }
    }
}

using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    class Dx01213 : BaseTests
    {

        private QuestionPage LaunchViaSearchLink(string sex, int age, string searchTerm, string titleToFind)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, sex, age);
            var questionInfoPage = searchPage.TypeSearchTextAndSelect(searchTerm, titleToFind);
            var questionPage = questionInfoPage.ClickIUnderstand();
            questionPage.VerifyQuestionPageLoaded();

            return questionPage;
        }

        [Test]
        public void NavigateToDispositionDx01213()
        {
            //0,2,2,0,0
            var questionPage = LaunchViaSearchLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "poison", "Accidental Poisoning/Inhalation");
            var outcomePage = questionPage.Answer(3) // No - trying to harm
                .Answer(3) //No - insulin
                .Answer(1) //Yes - know how much
                .Answer<OutcomePage>(1); //Yes - breathless

            outcomePage.VerifyHiddenField("Id", "Dx01213");

        }


    }
}

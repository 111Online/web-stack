using NHS111.Web.Functional.Utils;

namespace NHS111.Web.Functional.Tests
{
    using NUnit.Framework;
    
    [TestFixture]
    [Category("Covid")]
    public class CovidTests : BaseTests {
        [TestCase(TestScenerioSex.Female, 55, "PX113.3.PW1851.3", TestName = "NotCovidDisposition Above Pregnancy Age Female Adult")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "PX113.3.PW1851.3", TestName = "NotCovidDisposition Male Adult")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "PX117.1.PW1852.1", TestName = "NotCovidDisposition Male Child")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "PX117.1.PW1852.1", TestName = "NotCovidDisposition Below Pregnancy Age Female Child")]
        public void NotCovidDisposition(string gender, int ageGroup, string dispositionCode)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, gender, ageGroup, "LS17 7NZ");
            var questionPage = searchPage.ClickBannerDirectLink();

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "2")
                .Answer(2) // no
                .Answer(3) // no
                .Answer(3) // no
                .Answer<OutcomePage>(3); // no

            outcomePage.VerifyDispositionCode(dispositionCode);
        }

        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "PX119.0.PW1851.0", TestName = "NotCovidPregnancyDisposition Female Adult")]
        [TestCase(TestScenerioSex.Female, 11, "PX120.0.PW1852.0", TestName = "NotCovidPregnancyDisposition Female Child")]
        public void NotCovidPregnancyDisposition(string gender, int ageGroup, string dispositionCode)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, gender, ageGroup, "LS17 7NZ");
            var questionPage = searchPage.ClickBannerDirectLink();

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "2")
                .Answer(2) // no
                .Answer<OutcomePage>(2); // no

            outcomePage.VerifyDispositionCode(dispositionCode);
        }

        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "PX115.5.PW1851.5", "PW556.6800", TestName = "CovidMoreQuestions Female Adult")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "PX115.5.PW1851.5", "PW559.5200", TestName = "CovidMoreQuestions Male Adult")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "PX118.0.PW1852.0", "PW557.6600", TestName = "CovidMoreQuestions Female Child")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "PX118.0.PW1852.0", "PW560.5000", TestName = "CovidMoreQuestions Male Child")]
        public void CovidBreathlessInterstitial(string gender, int ageGroup, string dispositionCode, string furtherQuestionId)
        {
            var outcomePage = NavigateToInterstitial(gender, ageGroup, dispositionCode, furtherQuestionId);

            // interstitial
            outcomePage.VerifyDispositionCode(dispositionCode);

        }

        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "PX115.5.PW1851.5", "PW556.6800", TestName = "CovidMoreQuestions Female Adult")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "PX115.5.PW1851.5", "PW559.5200", TestName = "CovidMoreQuestions Male Adult")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "PX118.0.PW1852.0", "PW557.6600", TestName = "CovidMoreQuestions Female Child")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "PX118.0.PW1852.0", "PW560.5000", TestName = "CovidMoreQuestions Male Child")]
        public void BreathlessPathway(string gender, int ageGroup, string dispositionCode, string furtherQuestionId)
        {
            
            var outcomePage = NavigateToInterstitial(gender, ageGroup, dispositionCode, furtherQuestionId);
            // interstitial
            var furtherQuestionPage = outcomePage.ClickNext();

            // breathing pathway
            furtherQuestionPage.VerifyHiddenField("Id", furtherQuestionId);
        }

        private OutcomePage NavigateToInterstitial(string gender, int ageGroup, string dispositionCode, string furtherQuestionId)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, gender, ageGroup, "LS17 7NZ");
            var questionPage = searchPage.ClickBannerDirectLink();

            return questionPage.AnswerText("SymptomsStart_Day", "6")
               .Answer(2) // no
               .Answer(3) // no
               .Answer(3) // no
               .Answer<OutcomePage>(1); // yes
        }

    }
}

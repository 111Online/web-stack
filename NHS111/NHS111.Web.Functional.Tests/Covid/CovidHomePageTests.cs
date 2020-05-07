using NHS111.Web.Functional.Utils;

namespace NHS111.Web.Functional.Tests
{
    using NUnit.Framework;

    [TestFixture]
    [Category("Covid")]
    public class CovidHomePageTests : BaseTests
    {

        [TestCase(TestName = "Start symptom checker from Start now")]
        public void StartSymptomChecker()
        {
            var covidHomePage = TestScenarioPart.CovidHomePage(Driver);
            covidHomePage.Visit();
            covidHomePage.VerifyStartNowButton();
        }

        [TestCase(TestName = "Start symptom checker from Start now link")]
        public void StartSymptomCheckerLink()
        {
            var covidHomePage = TestScenarioPart.CovidHomePage(Driver);
            covidHomePage.Visit();
            covidHomePage.VerifyStartNowLink();
        }

        [TestCase(TestName = "Can go to NHS UK website")]
        public void NHSUKLink()
        {
            var covidHomePage = TestScenarioPart.CovidHomePage(Driver);
            covidHomePage.Visit();
            covidHomePage.VerifyNHSUKLink();
        }

        [TestCase(TestName = "Can go to isolation note")]
        public void IsolationNoteLink()
        {
            var covidHomePage = TestScenarioPart.CovidHomePage(Driver);
            covidHomePage.Visit();
            covidHomePage.VerifyIsoNoteLink();
        }
    }
}

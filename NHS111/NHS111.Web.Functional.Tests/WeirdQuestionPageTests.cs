using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    public class WeirdQuestionPageTests : BaseTests
    {
        [Test]
        public void WeirdQuestionTest_ViaCovidLinkNoCustomTextOnWeirdQuestionPage()
        {
            var guidedSelectionPage = TestScenarios.LaunchGuidedSelectionScenario(Driver, TestScenarioSex.Male, TestScenarioAgeGroups.Adult);
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection("Cough");

            weirdQuestionPage.VerifyWeirdQuestionContent(false);
        }

        [Test]
        public void WeirdQuestionTest_ViaCovidSearchNoCustomTextOnWeirdQuestionPage()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, TestScenarioAgeGroups.Adult);
            var guidedSelectionPage = searchPage.SearchByCovidTerm("covid");
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection("Cough");

            weirdQuestionPage.VerifyWeirdQuestionContent(false);
        }

        [Test]
        public void WeirdQuestionTest_ViaSearchCovidPathwayCustomTextOnWeirdQuestionPage()
        {
            var weirdQuestionPage = TestScenarios.LaunchQuestionInfoScenario(Driver, "Cough", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);
            weirdQuestionPage.VerifyWeirdQuestionContent(true);
        }

        [Test]
        public void WeirdQuestionTest_ViaSearchNonCovidPathwayNoCustomTextOnWeirdQuestionPage()
        {
            var weirdQuestionPage = TestScenarios.LaunchQuestionInfoScenario(Driver, "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);
            weirdQuestionPage.VerifyWeirdQuestionContent(false);
        }
    }
}

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
            var guidedSelectionPage = TestScenerios.LaunchGuidedSelectionScenario(Driver, TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection("Cough");

            weirdQuestionPage.VerifyWeirdQuestionContent(false);
        }

        [Test]
        public void WeirdQuestionTest_ViaCovidSearchNoCustomTextOnWeirdQuestionPage()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            var guidedSelectionPage = searchPage.SearchByCovidTerm("covid");
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection("Cough");

            weirdQuestionPage.VerifyWeirdQuestionContent(false);
        }

        [Test]
        public void WeirdQuestionTest_ViaSearchCovidPathwayCustomTextOnWeirdQuestionPage()
        {
            var weirdQuestionPage = TestScenerios.LaunchQuestionInfoScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            weirdQuestionPage.VerifyWeirdQuestionContent(true);
        }

        [Test]
        public void WeirdQuestionTest_ViaSearchNonCovidPathwayNoCustomTextOnWeirdQuestionPage()
        {
            var weirdQuestionPage = TestScenerios.LaunchQuestionInfoScenerio(Driver, "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            weirdQuestionPage.VerifyWeirdQuestionContent(false);
        }
    }
}

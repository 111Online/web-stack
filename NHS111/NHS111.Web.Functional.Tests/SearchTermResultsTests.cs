using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class SearchTermResultsTests : BaseTests
    {
        [TestCase("tummy pain", "Stomach ache or pain")] //abdo pain
        [TestCase("pain in my tooth", "Toothache, swelling and other dental problems")] //dental
        [TestCase("diorea", "Diarrhoea and vomiting")] // mispelling
        [TestCase("stomach ache", "Stomach ache or pain")] //synonym
        [TestCase("Head ache", "Headache or migraine")] //synonym
        [TestCase("diarhoea", "Diarrhoea")] //misspelling not on list
        [TestCase("vomitting", "Vomiting or nausea")] //misspelling
        [TestCase("Palpitations", "Palpitations")] //Appears in digital title only not description
        [TestCase("bloating", "Loss of bowel control")] //Appears in description only not title
        [TestCase("Choking", "Swallowed or breathed in an object")] //Appears in digital description only
        //following content updates to improve search for bleeding, pregnancy and asthma
        [TestCase("Wheezing", "Breathing problems")]
        [TestCase("Bleeding", "Bleeding from the bottom")]
        [TestCase("Bleeding", "Nosebleed")]
        [TestCase("asthma", "Breathing problems")]
        public void SearchTermResults_CommonTermsReturnExpectedResult(string term, string result)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 33);
            searchPage.SearchByTerm(term);
            searchPage.VerifyTermHits(result, 5);
        }

        /*
         * This test will rely on services being in the Pharmacy Whitelist from CCG Service
         */
        [TestCase]
        public void SearchTermResults_EmergencyPrescriptionsPilotArea()
        {
            // First check a postcode that should show EP does
            var searchPagePilot = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 33, "L1 2SA");
            searchPagePilot.SearchByTerm("emergency prescription");
            searchPagePilot.VerifyTermHits("Emergency Prescription", 1);
            
            // Then check a postcode that shouldn't show EP doesn't
            var searchPageNotPilot = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 33, "PO22 8PB");
            searchPageNotPilot.SearchByTerm("emergency prescription");
            searchPageNotPilot.VerifyTermNoHits("PW1827");
        }
    }
}

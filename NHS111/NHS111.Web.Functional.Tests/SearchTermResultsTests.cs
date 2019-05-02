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
    }
}

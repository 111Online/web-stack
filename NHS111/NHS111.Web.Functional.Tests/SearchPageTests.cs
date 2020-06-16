using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class SearchPageTests : BaseTests
    {
        [Test]
        public void SearchPage_Displays()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            searchPage.VerifyHeader();
        }

        [Test]
        public void SearchPage_SelectFirstResultStartsPathway()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            var questionInfoPage = searchPage.TypeSearchTextAndSelect("Bites and Stings");
            var questionPage = questionInfoPage.ClickIUnderstand();
            questionPage.VerifyQuestionPageLoaded();
        }

        [Test]
        public void SearchPage_TabbingOrder()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            searchPage.VerifyTabbingOrder("Bites or Stings");
        }

        [Test]
        [Ignore("Currently there is no validation on empty values. Ignoring unti confirmed this is correct.")]
        public void SearchPage_NoInputValidation()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            searchPage.SearchByTerm(string.Empty);
            searchPage.VerifyNoInputValidation();
        }

        [Test]
        public void SearchPage_ResultsWithApostropheHyphenAndBrackets()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            searchPage.SearchByTerm("'-/)}]Headache[{(\\");
            searchPage.VerifyTermHits("Headache or migraine", 1);
        }

        [Test]
        public void SearchPage_CategoryLinkShowsWithSearchResults()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            searchPage.SearchByTerm("Headache");
            searchPage.VerifyCategoriesLinkPresent();
        }

        [TestCase("cough")]
        [TestCase("high temperature")]
        [TestCase("breathlessness")]
        [TestCase("tiredness")]
        [TestCase("headache")]
        [TestCase("blocked nose")]
        [TestCase("loss of smell")]
        [TestCase("coughing")]
        [TestCase("hacking")]
        [TestCase("tickly cough")]
        [TestCase("breathing")]
        [TestCase("breathe")]
        [TestCase("wheeze")]
        [TestCase("wheezing")]
        [TestCase("wheezy")]
        [TestCase("gasping")]
        [TestCase("panting")]
        [TestCase("fever")]
        [TestCase("raised temperature")]
        [TestCase("37c")]
        [TestCase("38c")]
        [TestCase("39c")]
        [TestCase("40c")]
        [TestCase("ache")]
        [TestCase("head ache")]
        [TestCase("tired")]
        [TestCase("tiredness")]
        [TestCase("can't stay awake")]
        [TestCase("phlegm")]
        [TestCase("mucus")]
        [TestCase("sniffing")]
        [TestCase("smell")]
        [TestCase("taste")]
        [TestCase("anosmia")]
        [TestCase("chills")]
        [TestCase("feverish")]
        public void SearchPage_selecting_search_result_hit_using_Covid_search_term_launches_guided_selection(string searchTerm)
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            searchPage.SearchByTerm(searchTerm);

            if (searchPage.DataPathwayLinkPresentForPathway("PW1851"))
            {
                searchPage.ClickOnLinkWithDataPathway("PW1851");
                searchPage.VerifyOpensPage("NHS 111 Online - Covid Guided Selection Page");
            }
            else
            {
                Assert.Warn(string.Format("Corona virus result not found for term '{0}'", searchTerm));
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;
namespace NHS111.Web.Presentation.Builders.Tests
{
    [TestFixture()]
    public class KeywordCollectorTests
    {
        KeywordCollector _testKeywordCollector = new KeywordCollector();

        [Test()]
        public void Collect_multiple_keywords_Test()
        {
            var testJourneyModel = new JourneyViewModel();
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludedKeywordsString = "Test excluded keyword|AnotherTest excluded keyword";

            var testAnswer = new Answer(){Keywords = keywordsString, Title = "test", ExcludeKeywords = excludedKeywordsString};

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 2);
            Assert.True(result.CollectedKeywords.ExcludeKeywords.Count() == 2);
            Assert.AreEqual("Test keyword", result.CollectedKeywords.Keywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords.Keywords.ToArray()[1]);

            Assert.AreEqual("Test excluded keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest excluded keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[1]);

        }

        [Test()]
        public void Collect_single_keyword_Test()
        {
            var testJourneyModel = new JourneyViewModel();
            var keywordsString = "Test keyword";
            var excludedKeywordsString = "Test exclude keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test", ExcludeKeywords = excludedKeywordsString };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 1);
            Assert.True(result.CollectedKeywords.ExcludeKeywords.Count() == 1);
            Assert.AreEqual(keywordsString, result.CollectedKeywords.Keywords.ToArray()[0]);
            Assert.AreEqual(excludedKeywordsString, result.CollectedKeywords.ExcludeKeywords.ToArray()[0]);
        }


        [Test()]
        public void Collect_duplicate_keyword_Test()
        {
            var testJourneyModel = new JourneyViewModel() { CollectedKeywords = new KeywordBag(new List<string>() { "Test keyword" }, new List<string>() { "Test Excluded keyword" })};
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test", ExcludeKeywords = excludeKeywordsString };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 2);
            Assert.AreEqual("Test keyword", result.CollectedKeywords.Keywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords.Keywords.ToArray()[1]);

            Assert.AreEqual("Test Excluded keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest exclude keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[1]);
        }

        [Test()]
        public void Collect_keywords_ToExistingCollected_Test()
        {
            var testJourneyModel = new JourneyViewModel() { CollectedKeywords = new KeywordBag(new List<string>() { "Existing keyword" }, new List<string>() { "Existing Excluded keyword" })};
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test", ExcludeKeywords = excludeKeywordsString};

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 3);
            Assert.True(result.CollectedKeywords.ExcludeKeywords.Count() == 3);
            Assert.AreEqual("Existing keyword", result.CollectedKeywords.Keywords.ToArray()[0]);
            Assert.AreEqual("Test keyword", result.CollectedKeywords.Keywords.ToArray()[1]);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords.Keywords.ToArray()[2]);

            Assert.AreEqual("Existing Excluded keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[0]);
            Assert.AreEqual("Test Excluded keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[1]);
            Assert.AreEqual("AnotherTest exclude keyword", result.CollectedKeywords.ExcludeKeywords.ToArray()[2]);
        }

        [Test()]
        public void Collect_multiple_keywords_from_journeySteps_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testJourneySteps = new List<JourneyStep>(){new JourneyStep(){Answer = new Answer(){Keywords = keywordsString, ExcludeKeywords = excludeKeywordsString}}, new JourneyStep(){Answer = new Answer(){Keywords = "Keywords2", ExcludeKeywords = "excludeKeywords2"}}};

            var result = _testKeywordCollector.CollectFromJourneySteps(testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 3);
            Assert.True(result.ExcludeKeywords.Count() == 3);
            Assert.AreEqual("Test keyword", result.Keywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.Keywords.ToArray()[1]);
            Assert.AreEqual("Keywords2", result.Keywords.ToArray()[2]);

            Assert.AreEqual("Test Excluded keyword", result.ExcludeKeywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest exclude keyword", result.ExcludeKeywords.ToArray()[1]);
            Assert.AreEqual("excludeKeywords2", result.ExcludeKeywords.ToArray()[2]);
        }
        [Test()]
        public void Collect_multiple_keywords_from_journeySteps_removes_Dupes_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Exclude keyword|AnotherTest Exclude keyword";
            var testJourneySteps = new List<JourneyStep>() { 
                 new JourneyStep() { Answer = new Answer() { Keywords = keywordsString, ExcludeKeywords = excludeKeywordsString} }
                ,new JourneyStep() { Answer = new Answer() { Keywords = "Keywords2", ExcludeKeywords = "ExcludeKeywords2"} } 
                ,new JourneyStep() { Answer = new Answer() { Keywords = "Test keyword", ExcludeKeywords = "Test Exclude keyword"} }};

            var result = _testKeywordCollector.CollectFromJourneySteps(testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 3);
            Assert.True(result.ExcludeKeywords.Count() == 3);
            Assert.AreEqual("Test keyword", result.Keywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.Keywords.ToArray()[1]);
            Assert.AreEqual("Keywords2", result.Keywords.ToArray()[2]);

            Assert.AreEqual("Test Exclude keyword", result.ExcludeKeywords.ToArray()[0]);
            Assert.AreEqual("AnotherTest Exclude keyword", result.ExcludeKeywords.ToArray()[1]);
            Assert.AreEqual("ExcludeKeywords2", result.ExcludeKeywords.ToArray()[2]);
        }

        [Test()]
        public void ParseKeywords_emptyString_Test()
        {
            var keywordsString = string.Empty;
            var result = _testKeywordCollector.ParseKeywords(keywordsString);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 0);
        }

        [Test()]
        public void ParseKeywords_singlekeyword_Test()
        {
            var keywordsString = "Test keyword";
            var result = _testKeywordCollector.ParseKeywords(keywordsString);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 1);
            Assert.AreEqual(keywordsString, result.ToArray()[0]);
        }

        [Test()]
        public void ParseKeywords_multiplekeywords_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var result = _testKeywordCollector.ParseKeywords(keywordsString);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 2);
            Assert.AreEqual("Test keyword", result.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.ToArray()[1]);
        }

        [Test()]
        public void ParseKeywords_multiplekeywords_trimmed_Test()
        {
            var keywordsString = "Test keyword| AnotherTest keyword";
            var result = _testKeywordCollector.ParseKeywords(keywordsString);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 2);
            Assert.AreEqual("Test keyword", result.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.ToArray()[1]);
        }

        [Test]
        public void ConsolidateKeywords_WithNullOrEmptyKeywordBag_ReturnsEmptyCollection()
        {
            var sut = new KeywordCollector();
            var result = sut.ConsolidateKeywords(new KeywordBag());
            Assert.IsEmpty(result);
        }

        [Test]
        public void ConsolidateKeywords_WithKeywords_AndNoExcludes_ReturnsCorrectCollection()
        {
            var sut = new KeywordCollector();
            var keywords = new List<string>(){"Test keyword", "Another test keyword"};
            var result = sut.ConsolidateKeywords(new KeywordBag(keywords, new List<string>()));
            Assert.AreEqual(2, result.Count());
        }


        [Test]
        public void ConsolidateKeywords_WithKeywords_and_nonExcludable_keywords_ReturnsCorrectCollection()
        {
            var sut = new KeywordCollector();
            var keywords = new List<string>() { "Test keyword", "Another test keyword" };
            var excludeKeywords = new List<string>() { "Exclude keyword not in keyword list", "Another exclude keyword" };
            var result = sut.ConsolidateKeywords(new KeywordBag(keywords, excludeKeywords));
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Test keyword", result.ToArray()[0]);
            Assert.AreEqual("Another test keyword", result.ToArray()[1]);
        }
        [Test]
        public void ConsolidateKeywords_WithKeywords_and_Excludable_keywords_ReturnsCorrectCollection()
        {
            var sut = new KeywordCollector();
            var keywords = new List<string>() { "Test keyword", "Another test keyword" };
            var excludeKeywords = new List<string>() { "Test keyword", "Another exclude keyword" };
            var result = sut.ConsolidateKeywords(new KeywordBag(keywords, excludeKeywords));
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Another test keyword", result.ToArray()[0]);
        }

        [Test]
        public void ParseKeywords_WithNullOrEmpty_ReturnsEmptyCollection() {
            var sut = new KeywordCollector();
            var result = sut.ParseKeywords(null);

            Assert.IsEmpty(result);
            result = sut.ParseKeywords("");
            Assert.IsEmpty(result);
        }
    }
}

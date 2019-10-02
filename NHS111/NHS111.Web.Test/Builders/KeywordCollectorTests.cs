using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Parser;
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

            var testAnswer = new Answer(){ Keywords = keywordsString, Title = "test", ExcludeKeywords = excludedKeywordsString };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 2);
            Assert.True(result.CollectedKeywords.ExcludeKeywords.Count() == 2);
            Assert.AreEqual("Test keyword", result.CollectedKeywords.Keywords[0].Value);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords.Keywords[1].Value);
            Assert.AreEqual("Test excluded keyword", result.CollectedKeywords.ExcludeKeywords[0].Value);
            Assert.AreEqual("AnotherTest excluded keyword", result.CollectedKeywords.ExcludeKeywords[1].Value);

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
            Assert.AreEqual(keywordsString, result.CollectedKeywords.Keywords[0].Value);
            Assert.IsTrue(result.CollectedKeywords.Keywords[0].IsFromAnswer);
            Assert.AreEqual(excludedKeywordsString, result.CollectedKeywords.ExcludeKeywords[0].Value);
            Assert.IsTrue(result.CollectedKeywords.ExcludeKeywords[0].IsFromAnswer);

        }


        [Test()]
        public void Collect_duplicate_keyword_Test()
        {
            var testJourneyModel = new JourneyViewModel() { CollectedKeywords = new KeywordBag(new List<Keyword>() { new Keyword() { Value = "Test keyword" } }, new List<Keyword>() { new Keyword() { Value = "Test Excluded keyword" } })};
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test", ExcludeKeywords = excludeKeywordsString };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 2);
            Assert.AreEqual("Test keyword", result.CollectedKeywords.Keywords[0].Value);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords.Keywords[1].Value);

            Assert.AreEqual("Test Excluded keyword", result.CollectedKeywords.ExcludeKeywords[0].Value);
            Assert.AreEqual("AnotherTest exclude keyword", result.CollectedKeywords.ExcludeKeywords[1].Value);
        }

        [Test()]
        public void Collect_keywords_ToExistingCollected_Test()
        {
            var testJourneyModel = new JourneyViewModel() { CollectedKeywords = new KeywordBag(new List<Keyword>() { new Keyword() { Value = "Existing keyword" } }, new List<Keyword>() { new Keyword() { Value = "Existing Excluded keyword" } })};
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test", ExcludeKeywords = excludeKeywordsString};

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Keywords.Count() == 3);
            Assert.True(result.CollectedKeywords.ExcludeKeywords.Count() == 3);
            Assert.AreEqual("Existing keyword", result.CollectedKeywords.Keywords[0].Value);
            Assert.AreEqual("Test keyword", result.CollectedKeywords.Keywords[1].Value);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords.Keywords[2].Value);

            Assert.AreEqual("Existing Excluded keyword", result.CollectedKeywords.ExcludeKeywords[0].Value);
            Assert.AreEqual("Test Excluded keyword", result.CollectedKeywords.ExcludeKeywords[1].Value);
            Assert.AreEqual("AnotherTest exclude keyword", result.CollectedKeywords.ExcludeKeywords[2].Value);
        }

        [Test()]
        public void Collect_multiple_keywords_from_journeySteps_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testJourneySteps = new List<JourneyStep>(){new JourneyStep(){Answer = new Answer(){Keywords = keywordsString, ExcludeKeywords = excludeKeywordsString}}, new JourneyStep(){Answer = new Answer(){Keywords = "Keywords2", ExcludeKeywords = "excludeKeywords2"}}};

            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(null, testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 3);
            Assert.True(result.ExcludeKeywords.Count() == 3);
            Assert.AreEqual("Test keyword", result.Keywords[0].Value);
            Assert.AreEqual("AnotherTest keyword", result.Keywords[1].Value);
            Assert.AreEqual("Keywords2", result.Keywords[2].Value);

            Assert.AreEqual("Test Excluded keyword", result.ExcludeKeywords[0].Value);
            Assert.AreEqual("AnotherTest exclude keyword", result.ExcludeKeywords[1].Value);
            Assert.AreEqual("excludeKeywords2", result.ExcludeKeywords[2].Value);
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

            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(null, testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 3);
            Assert.True(result.ExcludeKeywords.Count() == 3);
            Assert.AreEqual("Test keyword", result.Keywords[0].Value);
            Assert.AreEqual("AnotherTest keyword", result.Keywords[1].Value);
            Assert.AreEqual("Keywords2", result.Keywords[2].Value);

            Assert.AreEqual("Test Exclude keyword", result.ExcludeKeywords[0].Value);
            Assert.AreEqual("AnotherTest Exclude keyword", result.ExcludeKeywords[1].Value);
            Assert.AreEqual("ExcludeKeywords2", result.ExcludeKeywords[2].Value);
        }

        [Test()]
        public void Collect_keywords_from_journeySteps_defaults_isfromanswer_to_true_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var excludeKeywordsString = "Test Excluded keyword|AnotherTest exclude keyword";
            var testJourneySteps = new List<JourneyStep>() { new JourneyStep() { Answer = new Answer() { Keywords = keywordsString, ExcludeKeywords = excludeKeywordsString } }, new JourneyStep() { Answer = new Answer() { Keywords = "Keywords2", ExcludeKeywords = "excludeKeywords2" } } };

            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(null, testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 3);
            Assert.True(result.ExcludeKeywords.Count() == 3);
            Assert.IsTrue(result.Keywords[0].IsFromAnswer);
            Assert.IsTrue(result.Keywords[1].IsFromAnswer);
            Assert.IsTrue(result.Keywords[2].IsFromAnswer);

            Assert.IsTrue(result.ExcludeKeywords[0].IsFromAnswer);
            Assert.IsTrue(result.ExcludeKeywords[1].IsFromAnswer);
            Assert.IsTrue(result.ExcludeKeywords[2].IsFromAnswer);
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
            Assert.AreEqual(keywordsString, result.ToArray()[0].Value);
            Assert.IsTrue(result.ToArray()[0].IsFromAnswer);
        }

        [Test()]
        public void ParseKeywords_multiplekeywords_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var result = _testKeywordCollector.ParseKeywords(keywordsString);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 2);
            Assert.AreEqual("Test keyword", result.ToArray()[0].Value);
            Assert.AreEqual("AnotherTest keyword", result.ToArray()[1].Value);
        }

        [Test()]
        public void ParseKeywords_multiplekeywords_trimmed_Test()
        {
            var keywordsString = "Test keyword| AnotherTest keyword";
            var result = _testKeywordCollector.ParseKeywords(keywordsString);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 2);
            Assert.AreEqual("Test keyword", result.ToArray()[0].Value);
            Assert.AreEqual("AnotherTest keyword", result.ToArray()[1].Value);
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
            var keywords = new List<Keyword>(){ new Keyword() { Value = "Test keyword" }, new Keyword() { Value = "Another test keyword" } };
            var result = sut.ConsolidateKeywords(new KeywordBag(keywords, new List<Keyword>()));
            Assert.AreEqual(2, result.Count());
        }


        [Test]
        public void ConsolidateKeywords_WithKeywords_and_nonExcludable_keywords_ReturnsCorrectCollection()
        {
            var sut = new KeywordCollector();
            var keywords = new List<Keyword>() { new Keyword() { Value = "Test keyword" }, new Keyword() { Value = "Another test keyword" } };
            var excludeKeywords = new List<Keyword>() { new Keyword() { Value = "Exclude keyword not in keyword list" }, new Keyword() { Value = "Another exclude keyword" } };
            var result = sut.ConsolidateKeywords(new KeywordBag(keywords, excludeKeywords));
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Test keyword", result.ToArray()[0]);
            Assert.AreEqual("Another test keyword", result.ToArray()[1]);
        }
        [Test]
        public void ConsolidateKeywords_WithKeywords_and_Excludable_keywords_ReturnsCorrectCollection()
        {
            var sut = new KeywordCollector();
            var keywords = new List<Keyword>() { new Keyword() { Value = "Test keyword" }, new Keyword() { Value = "Another test keyword" } };
            var excludeKeywords = new List<Keyword>() { new Keyword() { Value = "Test keyword" }, new Keyword() { Value = "Another exclude keyword" } };
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

        [Test()]
        public void ParseKeywords_passingFalseIsFromAnswerValue_Test()
        {
            var keywordsString = "Test keyword";
            var result = _testKeywordCollector.ParseKeywords(keywordsString, false);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 1);
            Assert.AreEqual(keywordsString, result.ToArray()[0].Value);
            Assert.IsFalse(result.ToArray()[0].IsFromAnswer);
        }

        [Test()]
        public void Collect_keywords_not_from_journey_steps_Test()
        {
            var keywordBag = new KeywordBag()
            {
                Keywords = new List<Keyword>()
                {
                    new Keyword() {Value = "keyword from answer", IsFromAnswer = true},
                    new Keyword() {Value = "keyword not from answer", IsFromAnswer = false},
                },
                ExcludeKeywords = new List<Keyword>()
                {
                    new Keyword() {Value = "exclude keyword from answer", IsFromAnswer = true},
                    new Keyword() {Value = "exclude keyword not from answer", IsFromAnswer = false},
                }
            };

            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(keywordBag, null);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 1);
            Assert.True(result.ExcludeKeywords.Count() == 1);
        }

        [Test()]
        public void Collect_keywords_not_from_journey_steps_null_keyword_bag_Test()
        {
            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(null, null);
            Assert.IsNotNull(result);
            Assert.True(!result.ExcludeKeywords.Any());
            Assert.True(!result.ExcludeKeywords.Any());
        }

        [Test()]
        public void Collect_keywords_not_from_journey_steps_empty_list_exclude_keywords_bag_Test()
        {
            var keywordBag = new KeywordBag()
            {
                Keywords = new List<Keyword>()
                {
                    new Keyword() {Value = "keyword from answer", IsFromAnswer = true},
                    new Keyword() {Value = "keyword not from answer", IsFromAnswer = false},
                }
            };

            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(keywordBag, null);
            Assert.IsNotNull(result);
            Assert.True(result.Keywords.Count() == 1);
            Assert.True(!result.ExcludeKeywords.Any());
        }

        [Test()]
        public void Collect_keywords_not_from_journey_steps_empty_list_keywords_bag_Test()
        {
            var keywordBag = new KeywordBag()
            {
                ExcludeKeywords = new List<Keyword>()
                {
                    new Keyword() {Value = "exclude keyword from answer", IsFromAnswer = true},
                    new Keyword() {Value = "exclude keyword not from answer", IsFromAnswer = false},
                }
            };

            var result = _testKeywordCollector.CollectKeywordsFromPreviousQuestion(keywordBag, null);
            Assert.IsNotNull(result);
            Assert.True(!result.Keywords.Any());
            Assert.True(result.ExcludeKeywords.Count() == 1);
        }
    }
}

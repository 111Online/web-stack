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
            var testAnswer = new Answer(){Keywords = keywordsString, Title = "test"};

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Count == 2);
            Assert.AreEqual("Test keyword", result.CollectedKeywords[0]);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords[1]);
        }

        [Test()]
        public void Collect_single_keyword_Test()
        {
            var testJourneyModel = new JourneyViewModel();
            var keywordsString = "Test keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test" };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Count == 1);
            Assert.AreEqual(keywordsString, result.CollectedKeywords[0]);
        }


        [Test()]
        public void Collect_duplicate_keyword_Test()
        {
            var testJourneyModel = new JourneyViewModel() { CollectedKeywords = new List<string>() { "Test keyword" } };
            var keywordsString = "Test keyword|AnotherTest keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test" };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Count == 2);
            Assert.AreEqual("Test keyword", result.CollectedKeywords[0]);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords[1]);
        }

        [Test()]
        public void Collect_keywords_ToExistingCollected_Test()
        {
            var testJourneyModel = new JourneyViewModel() { CollectedKeywords = new List<string>() { "Existing keyword" } };
            var keywordsString = "Test keyword|AnotherTest keyword";
            var testAnswer = new Answer() { Keywords = keywordsString, Title = "test" };

            var result = _testKeywordCollector.Collect(testAnswer, testJourneyModel);
            Assert.IsNotNull(result);
            Assert.True(result.CollectedKeywords.Count == 3);
            Assert.AreEqual("Existing keyword", result.CollectedKeywords[0]);
            Assert.AreEqual("Test keyword", result.CollectedKeywords[1]);
            Assert.AreEqual("AnotherTest keyword", result.CollectedKeywords[2]);
        }

        [Test()]
        public void Collect_multiple_keywords_from_journeySteps_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var testJourneySteps = new List<JourneyStep>(){new JourneyStep(){Answer = new Answer(){Keywords = keywordsString}}, new JourneyStep(){Answer = new Answer(){Keywords = "Keywords2"}}};

            var result = _testKeywordCollector.CollectFromJourneySteps(testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 3);
            Assert.AreEqual("Test keyword", result.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.ToArray()[1]);
             Assert.AreEqual("Keywords2", result.ToArray()[2]);
        }
        [Test()]
        public void Collect_multiple_keywords_from_journeySteps_removes_Dupes_Test()
        {
            var keywordsString = "Test keyword|AnotherTest keyword";
            var testJourneySteps = new List<JourneyStep>() { 
                 new JourneyStep() { Answer = new Answer() { Keywords = keywordsString } }
                ,new JourneyStep() { Answer = new Answer() { Keywords = "Keywords2" } } 
                ,new JourneyStep() { Answer = new Answer() { Keywords = "Test keyword" } }};

            var result = _testKeywordCollector.CollectFromJourneySteps(testJourneySteps);
            Assert.IsNotNull(result);
            Assert.True(result.Count() == 3);
            Assert.AreEqual("Test keyword", result.ToArray()[0]);
            Assert.AreEqual("AnotherTest keyword", result.ToArray()[1]);
            Assert.AreEqual("Keywords2", result.ToArray()[2]);
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
        public void ParseKeywords_WithNullOrEmpty_ReturnsEmptyCollection() {
            var sut = new KeywordCollector();

            var result = sut.ParseKeywords(null);

            Assert.IsEmpty(result);

            result = sut.ParseKeywords("");

            Assert.IsEmpty(result);
        }
    }
}

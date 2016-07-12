using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Business.Glossary.Api.Models;
using NHS111.Business.Glossary.Api.Services;
using NHS111.Domain.Glossary;
using NUnit.Framework;

namespace NHS111.Business.Glossary.Api.Services.Tests
{
    [TestFixture()]
    public class TermsServiceTests
    {

        private Mock<IDefinitionRepository> _definitionRepositorymock = new Mock<IDefinitionRepository>();

        public const string TEST_TERM = "TestTerm";
        public const string TEST_PHRASE = "Test Term Multiword";
        public const string TEST_SYNONYM = "SyonymTerm";

        private List<DefinedTerm> TEST_DEFINED_TERMS = new List<DefinedTerm>()
        {
            new DefinedTerm(){Term = TEST_TERM, Definition = "A definition for the Test Term" },
            new DefinedTerm(){Term = TEST_PHRASE, Definition = "A different definition for the Test Term Multiword" },
            new DefinedTerm(){Term = "TestTermWithSynonym", Definition = "A definition for the Test Term Multiword with Synonym", Synonyms = TEST_SYNONYM}
        };
        [SetUp]
        public void TermsServiceTestsSetup()
        {
            _definitionRepositorymock.Setup(r => r.List()).Returns(TEST_DEFINED_TERMS);
        }


        [Test()]
        public async void ListDefinedTerms_Returns_Terms_Test()
        {
            var termsService = new TermsService(_definitionRepositorymock.Object);
            var definitions = await termsService.ListDefinedTerms();

            Assert.IsNotNull(definitions);
            Assert.IsTrue(definitions.Any());
        }

        [Test()]
        public async void ListDefinedTerms_Returns_Terms_Returns_Synonyms()
        {
            var termsService = new TermsService(_definitionRepositorymock.Object);
            var definitions = await termsService.ListDefinedTerms();

            Assert.IsNotNull(definitions);
            Assert.AreEqual(4, definitions.Count());
            Assert.AreEqual(TEST_SYNONYM, definitions.Last().Term);

        }

        [Test()]
        public async void FindContainedTerms_Returns_all_Matched_Terms()
        {
            string testinput = String.Format("A sentance containing {0} along with a {1} contained mid sentance", TEST_TERM, TEST_PHRASE);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(2, definitions.Count());
            Assert.IsTrue(definitions.Any(d => d.Term == TEST_TERM));
            Assert.IsTrue(definitions.Any(d => d.Term == TEST_PHRASE));
        }

        [Test()]
        public async void FindContainedTerms_Returns_Matched_Term_at_end_of_text()
        {
            string testinput = String.Format("A sentance containing term {0}", TEST_TERM);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.All(d => d.Term == TEST_TERM));
        }

        [Test()]
        public async void FindContainedTerms_Returns_Matched_Term_at_start_of_text()
        {
            string testinput = String.Format("{0} a sentance containing term", TEST_TERM);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.All(d => d.Term == TEST_TERM));
        }

        [Test()]
        public async void FindContainedTerms_Returns_Matched_Phrase_at_end_of_text()
        {
            string testinput = String.Format("A sentance containing phrase {0}", TEST_PHRASE);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.All(d => d.Term == TEST_PHRASE));
        }


        [Test()]
        public async void FindContainedTerms_Returns_Matched_Phrase_at_start_of_text()
        {
            string testinput = String.Format("{0} a sentance containing phrase ", TEST_PHRASE);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.All(d => d.Term == TEST_PHRASE));
        }

        [Test()]
        public async void FindContainedTerms_Returns_all_Matched_Synonyms()
        {
            string testinput = String.Format("A sentance containing the Synonym {0} along with some other text", TEST_SYNONYM);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.Any(d => d.Term == TEST_SYNONYM));
            Assert.IsTrue(definitions.Any(d => d.Definition == "A definition for the Test Term Multiword with Synonym"));
        }

        [Test()]
        public async void FindContainedTerms_Returns_matched_terms_with_punctuation()
        {
            string testinput = String.Format("A sentance containing the Term {0}! Which was punctuated.", TEST_TERM);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.Any(d => d.Term == TEST_TERM));
        }

        [Test()]
        public async void FindContainedTerms_Returns_matched_phrase_with_punctuation()
        {
            string testinput = String.Format("A sentance containing the Phrase {0}? Which was punctuated.", TEST_PHRASE);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.AreEqual(1, definitions.Count());
            Assert.IsTrue(definitions.Any(d => d.Term == TEST_PHRASE));
        }

        [Test()]
        public async void FindContainedTerms_Does_not_return_Terms_within_Other_Terms()
        {
            string testinput = String.Format("A sentance containing the Term {0}s Which is contained within another word.", TEST_TERM);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.IsFalse(definitions.Any());
        }

        [Test()]
        public async void FindContainedTerms_Does_not_return_Phrases_within_Other_Terms()
        {
            string testinput = String.Format("A sentance containing the Phrase {0}s Which is contained within another word.", TEST_PHRASE);
            var definitions = await SetupAndRunFindContainedTerms(testinput);

            Assert.IsNotNull(definitions);
            Assert.IsFalse(definitions.Any());
        }

        private async Task<IEnumerable<NHS111.Models.Models.Domain.DefinedTerm>> SetupAndRunFindContainedTerms(string inputString)
        {
            var termsService = new TermsService(_definitionRepositorymock.Object);
            return await termsService.FindContainedTerms(inputString);
        }

  
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Controllers;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class QuestionViewModelBuilderTests
    {
        Mock<IOutcomeViewModelBuilder> _outcomeViewModelBuilder;
        Mock<IJustToBeSafeFirstViewModelBuilder> _justToBeSafeFirstViewModelBuilder;
        Mock<IConfiguration> _configuration;
        Mock<IMappingEngine> _mappingEngine;
        Mock<ISymptomDicriminatorCollector> _symptomDicriminatorCollector;
        Mock<IKeywordCollector> _keywordCollector;
        private JourneyViewModelBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _outcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();
            _justToBeSafeFirstViewModelBuilder = new Mock<IJustToBeSafeFirstViewModelBuilder>();
            _mappingEngine = new Mock<IMappingEngine>();
            _keywordCollector = new Mock<IKeywordCollector>();
            _symptomDicriminatorCollector = new Mock<ISymptomDicriminatorCollector>();
            _sut = new JourneyViewModelBuilder(_outcomeViewModelBuilder.Object,
                _mappingEngine.Object, _symptomDicriminatorCollector.Object, _keywordCollector.Object, _justToBeSafeFirstViewModelBuilder.Object);
        }

        [Test]
        public void BuildPreviousQuestion_with_keywords_on_previous_answers_retains_keywords()
        {
            var journey = new JourneyViewModel
            {
                JourneyJson =
                    JsonConvert.SerializeObject(new Journey()
                    {
                        Steps = new List<JourneyStep>()
                        {
                            new JourneyStep() { QuestionId = "1", Answer = new Answer() { Keywords = "keyword 1|keyword 2", ExcludeKeywords = "" } },
                            new JourneyStep() { QuestionId = "2" }
                        }
                    }),
                PreviousStateJson = JsonConvert.SerializeObject(new Dictionary<string, string>()),
                CollectedKeywords = new KeywordBag()
                {
                    Keywords = new List<Keyword>()
                    {
                        new Keyword()
                        {
                            Value = "non journey step keyword",
                            IsFromAnswer = false
                        }
                    },
                    ExcludeKeywords = new List<Keyword>()
                    {
                        new Keyword()
                        {
                            Value = "non journey step exclude keyword",
                            IsFromAnswer = false
                        }
                    },
                }
            };
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.JourneyViewModelMapper>());
            _mappingEngine.Setup(x => x.Mapper).Returns(Mapper.Instance);

            var result = _sut.BuildPreviousQuestion(null, journey);

            //Assert
            Assert.AreEqual(3, result.CollectedKeywords.Keywords.Count);
            Assert.AreEqual(1, result.CollectedKeywords.ExcludeKeywords.Count);
        }
    }
}

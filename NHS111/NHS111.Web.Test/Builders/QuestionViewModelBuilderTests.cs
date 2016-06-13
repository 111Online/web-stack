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
        Mock<IRestfulHelper> _restfulHelper;
        Mock<IConfiguration> _configuration;
        Mock<IMappingEngine> _mappingEngine;
        Mock<ISymptomDicriminatorCollector> _symptomDicriminatorCollector;
        Mock<IKeywordCollector> _keywordCollector;
        private QuestionViewModelBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _outcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();
            _justToBeSafeFirstViewModelBuilder = new Mock<IJustToBeSafeFirstViewModelBuilder>();
            _restfulHelper = new Mock<IRestfulHelper>();
            _configuration = new Mock<IConfiguration>();
            _mappingEngine = new Mock<IMappingEngine>();
            _keywordCollector = new Mock<IKeywordCollector>();
            _symptomDicriminatorCollector = new Mock<ISymptomDicriminatorCollector>();
            _sut = new QuestionViewModelBuilder(_outcomeViewModelBuilder.Object,
                _justToBeSafeFirstViewModelBuilder.Object, _restfulHelper.Object, _configuration.Object,
                _mappingEngine.Object, _symptomDicriminatorCollector.Object, _keywordCollector.Object);
        }

        [Test]
        public async void BuildGender_valid_title_returns_pathway_numbers()
        {
            _configuration.Setup(x => x.GetBusinessApiPathwayNumbersUrl(It.IsAny<string>())).Returns("{0}");
            _restfulHelper.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult("PW111, PW112"));
            var result = await _sut.BuildGender(It.IsAny<string>());

            //Assert
            Assert.AreEqual(typeof (JourneyViewModel), result.GetType());
            Assert.AreEqual(result.PathwayNo, "PW111, PW112");
        }

        [Test]
        public async void BuildGender_invalid_title_returns_null()
        {
            _configuration.Setup(x => x.GetBusinessApiPathwayNumbersUrl(It.IsAny<string>())).Returns("{0}");
            _restfulHelper.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));
            
            var result = await _sut.BuildGender(It.IsAny<string>());
            
            //Assert
            Assert.AreEqual(typeof (JourneyViewModel), result.GetType());
            Assert.AreEqual(result.PathwayNo, string.Empty);
        }

        [Test]
        public async void BuildPreviousQuestion_with_keywords_on_previous_answers_retains_keywords()
        {
            _sut = new QuestionViewModelBuilder(_outcomeViewModelBuilder.Object,
                _justToBeSafeFirstViewModelBuilder.Object, _restfulHelper.Object, _configuration.Object,
                _mappingEngine.Object, _symptomDicriminatorCollector.Object, new KeywordCollector());

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
            _configuration.Setup(x => x.GetBusinessApiQuestionByIdUrl(It.IsAny<string>(), It.IsAny<string>())).Returns("http://someapiendpoint");
            _restfulHelper.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.JourneyViewModelMapper>());
            _mappingEngine.Setup(x => x.Mapper).Returns(Mapper.Instance);

            var result = await _sut.BuildPreviousQuestion(journey);

            //Assert
            Assert.AreEqual(3, result.CollectedKeywords.Keywords.Count);
            Assert.AreEqual(1, result.CollectedKeywords.ExcludeKeywords.Count);
        }
    }
}

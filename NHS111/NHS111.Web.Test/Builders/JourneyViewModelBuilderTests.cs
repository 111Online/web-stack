using AutoMapper;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Validators;
using NHS111.Utils.Parser;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class JourneyViewModelBuilderTests
    {
        Mock<IOutcomeViewModelBuilder> _outcomeViewModelBuilder;
        Mock<IJustToBeSafeFirstViewModelBuilder> _justToBeSafeFirstViewModelBuilder;
        Mock<IMappingEngine> _mappingEngine;
        Mock<ISymptomDiscriminatorCollector> _symptomDicriminatorCollector;
        Mock<IPostCodeAllowedValidator> _postCodeAllowedValidator;
        private JourneyViewModelBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _outcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();
            _justToBeSafeFirstViewModelBuilder = new Mock<IJustToBeSafeFirstViewModelBuilder>();
            _mappingEngine = new Mock<IMappingEngine>();
            _symptomDicriminatorCollector = new Mock<ISymptomDiscriminatorCollector>();
            _postCodeAllowedValidator = new Mock<IPostCodeAllowedValidator>();
            _sut = new JourneyViewModelBuilder(_outcomeViewModelBuilder.Object,
                _mappingEngine.Object, _symptomDicriminatorCollector.Object, new KeywordCollector(), _justToBeSafeFirstViewModelBuilder.Object, _postCodeAllowedValidator.Object);
        }
        /*
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
        */
        [Test]
        [Category("Integration")]
        public void BuildPreviousQuestion_with_keywords_on_previous_answers_retains_keywords()
        {
            var journey = new Journey()
            {
                Steps = new List<JourneyStep>()
                {
                    new JourneyStep() { QuestionId = "1", Answer = new Answer() { Keywords = "keyword 1|keyword 2", ExcludeKeywords = "" }, State = "{'PATIENT_AGE':'20'}"},
                    new JourneyStep() { QuestionId = "2",State = "{'PATIENT_AGE':'20'}" }
                }
            };
            var questionModel = new QuestionViewModel
            {
                Journey = journey,
                JourneyJson = JsonConvert.SerializeObject(journey),
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
            var result = _sut.BuildPreviousQuestion(null, questionModel);


            Assert.IsNotNull(result.CollectedKeywords);
            Assert.IsNotNull(result.CollectedKeywords.Keywords);
            Assert.AreEqual(3, result.CollectedKeywords.Keywords.Count);
            Assert.AreEqual(1, result.CollectedKeywords.ExcludeKeywords.Count);
        }
    }
}

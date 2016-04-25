using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
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
        private QuestionViewModelBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _outcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();
            _justToBeSafeFirstViewModelBuilder = new Mock<IJustToBeSafeFirstViewModelBuilder>();
            _restfulHelper = new Mock<IRestfulHelper>();
            _configuration = new Mock<IConfiguration>();
            _mappingEngine = new Mock<IMappingEngine>();
            _symptomDicriminatorCollector = new Mock<ISymptomDicriminatorCollector>();
            _sut = new QuestionViewModelBuilder(_outcomeViewModelBuilder.Object,
                _justToBeSafeFirstViewModelBuilder.Object, _restfulHelper.Object, _configuration.Object,
                _mappingEngine.Object, _symptomDicriminatorCollector.Object);
        }

        [Test]
        public async void BuildGender_valid_title_returns_pathway_numbers()
        {
            _configuration.SetupGet(x => x.BusinessApiPathwayNumbersUrl).Returns("{0}");
            _restfulHelper.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult("PW111, PW112"));
            var result = await _sut.BuildGender(It.IsAny<string>());

            //Assert
            Assert.AreEqual(typeof (JourneyViewModel), result.GetType());
            Assert.AreEqual(result.PathwayNo, "PW111, PW112");
        }

        [Test]
        public async void BuildGender_invalid_title_returns_null()
        {
            _configuration.SetupGet(x => x.BusinessApiPathwayNumbersUrl).Returns("{0}");
            _restfulHelper.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));
            var result = await _sut.BuildGender(It.IsAny<string>());

            //Assert
            Assert.AreEqual(typeof (JourneyViewModel), result.GetType());
            Assert.AreEqual(result.PathwayNo, string.Empty);
        }
    }
}

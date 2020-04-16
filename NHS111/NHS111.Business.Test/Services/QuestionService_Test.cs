
using System;
using NHS111.Business.Services;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Business.Builders;
using NHS111.Business.Test.Builders;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Business.Caching;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using NHS111.Utils.Parser;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.Test.Services
{
    [TestFixture]
    public class QuestionService_Test
    {
        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IRestClient> _restClient;

        private Mock<IAnswersForNodeBuilder> _answersForNodeBuilder;
        private Mock<IModZeroJourneyStepsBuilder> _modZeroJourneyStepsBuilder;
        private Mock<IKeywordCollector> _keywordCollector;
        private Mock<ICareAdviceService> _careAdviceService;
        private Mock<ICareAdviceTransformer> _careAdviceTransformer;
        private Mock<ICacheManager<string, string>> _cacheManagerMock;
        private ICacheStore _cacheStoreMock;
        private IRestResponse<QuestionWithAnswers> _mockQuestionRestResponse; 

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();
            _answersForNodeBuilder = new Mock<IAnswersForNodeBuilder>();
            _modZeroJourneyStepsBuilder = new Mock<IModZeroJourneyStepsBuilder>();
            _keywordCollector = new Mock<IKeywordCollector>();
            _careAdviceService = new Mock<ICareAdviceService>();
            _careAdviceTransformer = new Mock<ICareAdviceTransformer>();
            _restClient = new Mock<IRestClient>();
            _cacheManagerMock = new Mock<ICacheManager<string, string>>();
            _cacheStoreMock = new RedisCacheStore(_cacheManagerMock.Object);
            _mockQuestionRestResponse = new RestResponse<QuestionWithAnswers>(){Data = new QuestionWithAnswersBuilder("idQ2", "This is a test question?").Build() };
        }

        [Test]
        public async void GetQuestion_should_return_a_question_by_id()
        {
            //Arrange
            var id = "idQ1";
            ArrageDomainRequestMock(_mockQuestionRestResponse);
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);
            //Act
            var result = await sut.GetQuestion(id);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiQuestionUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));
        }

        [Test]
        public async void GetQuestion_should_return_a_question_by_id_before_adding_to_cache()
        {
            //Arrange
            var id = "idQ1";
            var expectedCacheKey = new QuestionWithAnswersCacheKey(id);


            ArrageDomainRequestMock(_mockQuestionRestResponse);
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);

           
            //Act
            var result = await sut.GetQuestion(id);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiQuestionUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));

        }

        [Test]
        public async void GetFirstQuestion_should_return_a_question_by_pathway_id()
        {
            //Arrange
            var pathwayId = "idPW1";
            ArrageDomainRequestMock( _mockQuestionRestResponse);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);
            //Act
            var result = await sut.GetFirstQuestion(pathwayId);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiFirstQuestionUrl(pathwayId), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));
        }


        [Test]
        public async void GetFirstQuestion_should_return_a_question_by_pathway_id_before_adding_to_cache()
        {
            //Arrange
            var pathwayId = "idPW1";
            ArrageDomainRequestMock(_mockQuestionRestResponse);
            var expectedCacheKey = new QuestionWithAnswersByPathwayCacheKey(pathwayId);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);
            //Act
            var result = await sut.GetFirstQuestion(pathwayId);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiFirstQuestionUrl(pathwayId), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));
        }

        [Test]
        public async void GetQuestion_should_return_following_question()
        {
            //Arrange

            var id = "idQ1";
            var nodeLabel = "Question";
            var answer = "no";

            ArrageDomainRequestMock( _mockQuestionRestResponse);
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetNextQuestion(id, nodeLabel, answer);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiNextQuestionUrl(id, nodeLabel), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));
        }

        [Test]
        public async void GetNextQuestion_should_return_following_question_before_adding_to_cache()
        {
            //Arrange

            var id = "idQ1";
            var nodeLabel = "Question";
            var answer = "no";
            var expectedCacheKey = new QuestionWithAnswersCacheKey(id, nodeLabel, answer);
         
            ArrageDomainRequestMock(_mockQuestionRestResponse);
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object, 
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);


            //Act
            var result = await sut.GetNextQuestion(id, nodeLabel, answer);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiNextQuestionUrl(id, nodeLabel), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));
        }

        [Test]
        public async void GetNextQuestion_should_not_call_restClient_if_present_in_cache()
        {
            //Arrange

            var id = "idQ1";
            var nodeLabel = "Question";
            var answer = "no";
            var expectedCacheKey = new QuestionWithAnswersCacheKey(id, nodeLabel, answer);

            ArrageDomainRequestMock(_mockQuestionRestResponse);
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(JsonConvert.SerializeObject(_mockQuestionRestResponse.Data));
            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);


            //Act
            var result = await sut.GetNextQuestion(id, nodeLabel, answer);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Never);
            _cacheManagerMock.Verify(x => x.Read(expectedCacheKey.CacheKey), Times.Once);
            _configuration.Verify(x => x.GetDomainApiNextQuestionUrl(id, nodeLabel), Times.Never);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Never);
            Assert.That(result.Question.Title, Is.EqualTo(_mockQuestionRestResponse.Data.Question.Title));
        }

        [Test]
        public async void GetNextQuestion_should_not_add_to_cache_for_null_restclient_response()
        {
            //Arrange

            var id = "idQ1";
            var nodeLabel = "Question";
            var answer = "no";
            var expectedCacheKey = new QuestionWithAnswersCacheKey(id, nodeLabel, answer);

            ArrageDomainRequestMock( new RestResponse<QuestionWithAnswers>(){Data = null});
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);
            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);


            //Act
            var result = await sut.GetNextQuestion(id, nodeLabel, answer);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Never);
            _configuration.Verify(x => x.GetDomainApiNextQuestionUrl(id, nodeLabel), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
        }

        [Test]
        public async void GetNextQuestion_should_not_add_to_cache_for_Empty_restclient_response()
        {
            //Arrange

            var id = "idQ1";
            var nodeLabel = "Question";
            var answer = "no";
            var expectedCacheKey = new QuestionWithAnswersCacheKey(id, nodeLabel, answer);

            ArrageDomainRequestMock( new RestResponse<QuestionWithAnswers>() { Data = new QuestionWithAnswersBuilder().Build()});
            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new QuestionService(_configuration.Object, _restClient.Object, _answersForNodeBuilder.Object,
                _modZeroJourneyStepsBuilder.Object, _keywordCollector.Object, _careAdviceService.Object, _careAdviceTransformer.Object, _cacheStoreMock);


            //Act
            var result = await sut.GetNextQuestion(id, nodeLabel, answer);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Never);
            _configuration.Verify(x => x.GetDomainApiNextQuestionUrl(id, nodeLabel), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>()), Times.Once);
        }

       

        private void ArrageDomainRequestMock(IRestResponse<QuestionWithAnswers> restClientResponse)
        {
            _restClient.Setup(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>())).ReturnsAsync(restClientResponse);
        }

        //[Test]
        //public async void should_return_first_jtbs_question()
        //{
        //    //Arrange

        //    var url = "http://mytest.com/";
        //    var id = "1";
        //    var resultString = "is this the first jtbs question?";

        //    _configuration.Setup(x => x.GetDomainApiJustToBeSafeQuestionsFirstUrl(id)).Returns(url);
        //    _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

        //    var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

        //    //Act
        //    var result = await sut.GetJustToBeSafeQuestionsFirst(id);

        //    //Assert 
        //    _configuration.Verify(x => x.GetDomainApiJustToBeSafeQuestionsFirstUrl(id), Times.Once);
        //    _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
        //    Assert.That(result, Is.EqualTo(resultString));
        //}

        //[Test]
        //public async void should_return_next_jtbs_question()
        //{
        //    //Arrange

        //    var url = "http://mytest.com/";
        //    var pathwayId = "qId1";
        //    var resultString = "is this next jtbs question?";

        //    var answeredQuestionIds = new List<string> { "1", "2", "3" };
        //    var multipleChoice = true;
        //    var selectedQuestionId = "qId10";

        //    _configuration.Setup(x => x.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId)).Returns(url);
        //    _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

        //    var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

        //    //Act
        //    var result = await sut.GetJustToBeSafeQuestionsNext(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId);

        //    //Assert 
        //    _configuration.Verify(x => x.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId), Times.Once);
        //    _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
        //    Assert.That(result, Is.EqualTo(resultString));
        //}
    }
}
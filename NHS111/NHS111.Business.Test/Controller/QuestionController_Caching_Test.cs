using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using NHS111.Business.Api.Controllers;
using NHS111.Business.Builders;
using NHS111.Business.Services;
using NHS111.Business.Test.Builders;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Test.Controller
{
    [TestFixture]
    public class QuestionController_Caching_Test
    {
        private Mock<IQuestionService> _questionService;
        private Mock<IQuestionTransformer> _questionTransformer;
        private Mock<IAnswersForNodeBuilder> _answersForNodeBuilder;
        private Mock<ICacheManager<string, string>> _cacheManager;
        private QuestionController _sut;
      
        private string pathwayId = "PW123MaleAdult";
        private string nodeId = "PW123.1200";
        private string answer = "yes";
        private string _expectedCacheKey;

        private bool _isRelease = false;

        [SetUp]
        public void SetUp()
        {
            _questionService = new Mock<IQuestionService>();
            _questionTransformer = new Mock<IQuestionTransformer>();
            _answersForNodeBuilder = new Mock<IAnswersForNodeBuilder>();

            _cacheManager = new Mock<ICacheManager<string, string>>();
            _cacheManager.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<string>()));

            _sut = new QuestionController(_questionService.Object, _questionTransformer.Object,
                _answersForNodeBuilder.Object, _cacheManager.Object);

           

            _expectedCacheKey = string.Format("{0}-{1}-{2}", pathwayId, nodeId, answer);
#if !DEBUG
            _isRelease = true;
#endif
        }

        [Test]
        public async void should_return_a_question_before_adding_to_cache()
        {

            if (!_isRelease) Assert.Ignore("This test must be run in release mode");

             var questionResult = new QuestionWithAnswersBuilder("1", "Test").AddAnswer("yes").Build();
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(questionResult));
              
            _questionTransformer.Setup(x => x.AsQuestionWithAnswers(It.IsAny<QuestionWithAnswers>())).Returns(questionResult);

            var result = await _sut.GetNextNode(pathwayId, NodeType.Question, nodeId, "", answer);

            _cacheManager.Verify(c => c.Set(_expectedCacheKey, JsonConvert.SerializeObject(questionResult)), Times.Once);
            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<QuestionWithAnswers>(result.Content);
        }

        [Test]
        public async void should_not_set_cache_when_questionService_returns_null()
        {
            if(!_isRelease) Assert.Ignore("This test must be run in release mode");
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(null);

            JsonResult<QuestionWithAnswers> result;
            try
            {
                result = await _sut.GetNextNode(pathwayId, NodeType.Question, nodeId, "", answer);
            }
            catch (Exception e)
            {
                
            }
            _cacheManager.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Test]
        public async void should_not_set_cache_when_questionService_returns_EmptyResponse()
        {
            if (!_isRelease) Assert.Ignore("This test must be run in release mode");
            var questionResult = new QuestionWithAnswersBuilder().Build();
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(questionResult);

            JsonResult<QuestionWithAnswers> result;
            try
            {
                result = await _sut.GetNextNode(pathwayId, NodeType.Question, nodeId, "", answer);
            }
            catch (Exception e)
            {

            }
            _cacheManager.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


    }

   
}

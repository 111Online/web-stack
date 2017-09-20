using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using NHS111.Business.Api.Controllers;
using NHS111.Business.Builders;
using NHS111.Business.Services;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Test.Controller
{
    [TestFixture]
    public class QuestionController_Test
    {
        private Mock<IQuestionService> _questionService;
        private Mock<IQuestionTransformer> _questionTransformer;
        private Mock<IAnswersForNodeBuilder> _answersForNodeBuilder;
        private Mock<ICacheManager<string, string>> _cacheManager;
        private QuestionController _sut;
        
        [SetUp]
        public void SetUp()
        {
            _questionService = new Mock<IQuestionService>();
            _questionTransformer = new Mock<IQuestionTransformer>();
            _answersForNodeBuilder = new Mock<IAnswersForNodeBuilder>();
            _cacheManager = new Mock<ICacheManager<string, string>>();
            _sut = new QuestionController(_questionService.Object, _questionTransformer.Object, _answersForNodeBuilder.Object, _cacheManager.Object);
        }

        [Test]
        public async void should_return_a_question_with_dead_end()
        {
            var question = new QuestionWithAnswers()
            {
                Question = new Question(),
                Labels  = new []
                {
                    "DeadEndJump"
                },
                State = new Dictionary<string, string>()
            };
            var json = JsonConvert.SerializeObject(question);
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(json.AsHttpResponse()));

            _questionTransformer.Setup(x => x.AsQuestionWithDeadEnd(It.IsAny<string>())).Returns(json);

            var result = await _sut.GetNextNode("1", NodeType.Question,  "2", "", "yes");

            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<QuestionWithDeadEnd>(JsonConvert.DeserializeObject<QuestionWithDeadEnd>(await result.Content.ReadAsStringAsync()));
        }

        [Test]
        public async void should_return_a_question_with_answers()
        {
            var question = new QuestionWithAnswers()
            {
                Question = new Question(),
                Labels = new[]
                {
                    "Question"
                },
                State = new Dictionary<string, string>()
            };
            var json = JsonConvert.SerializeObject(question);
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(json.AsHttpResponse()));

            _questionTransformer.Setup(x => x.AsQuestionWithAnswers(It.IsAny<string>())).Returns(json);

            var result = await _sut.GetNextNode("1", NodeType.Question,  "2", "", "yes");

            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<QuestionWithAnswers>(JsonConvert.DeserializeObject<QuestionWithAnswers>(await result.Content.ReadAsStringAsync()));
        }
    }
}

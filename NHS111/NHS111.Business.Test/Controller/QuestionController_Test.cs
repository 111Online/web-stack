using Moq;
using NHS111.Business.Api.Controllers;
using NHS111.Business.Builders;
using NHS111.Business.Services;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;
using NHS111.Utils.Cache;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                Labels = new[]
                {
                    "DeadEndJump"
                },
                State = new Dictionary<string, string>()
            };
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(question));

            //_questionTransformer.Setup(x => x.AsQuestionWithDeadEnd(It.IsAny<QuestionWithAnswers>())).Returns(question);

            var result = await _sut.GetNextNode("1", NodeType.Question, "2", "", "yes");

            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            //Assert.IsInstanceOf<QuestionWithDeadEnd>(JsonConvert.DeserializeObject<QuestionWithDeadEnd>(await result.Content.ReadAsStringAsync()));
            Assert.IsTrue(result.Content.Labels.Contains("DeadEndJump"));
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
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(question));

            _questionTransformer.Setup(x => x.AsQuestionWithAnswers(It.IsAny<QuestionWithAnswers>())).Returns(question);

            var result = await _sut.GetNextNode("1", NodeType.Question, "2", "", "yes");

            _questionService.Verify(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<QuestionWithAnswers>(result.Content);
        }

        [Test]
        public async void GetNextNode_WithKeywordsOnReadNodes_RecursivelyAppendsKeywordsToNextQuestion()
        {
            var node1 = new QuestionWithAnswers
            {
                Question = new Question { Title = "Read1" },
                Answers = new List<Answer>() {
                    new Answer {}
                },
                Labels = new[] {
                    "Read"
                },
                State = new Dictionary<string, string>()
            };
            var node2 = new QuestionWithAnswers
            {
                Question = new Question { Title = "Read2" },
                Answers = new List<Answer>() {
                    new Answer {}
                },
                Labels = new[] {
                    "Read"
                },
                State = new Dictionary<string, string>()
            };
            var node3 = new QuestionWithAnswers
            {
                Question = new Question { Title = "Q1" },
                Answers = new List<Answer>() {
                    new Answer {}
                },
                Labels = new[] {
                    "Question"
                },
                State = new Dictionary<string, string>()
            };

            _questionTransformer.Setup(x => x.AsQuestionWithAnswers(It.IsAny<QuestionWithAnswers>())).Returns(node3);
            _answersForNodeBuilder.Setup(x => x.SelectAnswer(It.IsAny<IEnumerable<Answer>>(), It.IsAny<string>()))
                .Returns(new Answer { Keywords = "kw1", ExcludeKeywords = "exclude1" });
            _questionService.SetupSequence(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(node1))
                .Returns(Task.FromResult(node2))
                .Returns(Task.FromResult(node3));

            var result = await _sut.GetNextNode("1", NodeType.Question, "2", "{}", "yes");
            var deserialisedResult = result.Content;
            Assert.IsTrue(deserialisedResult.Answers.First().Keywords.Contains("kw1"));
        }

        [Test]
        public async void question_state_contains_system_variables()
        {
            var question = new QuestionWithAnswers()
            {
                Question = new Question(),
                Labels = new[]
                {
                    "Question"
                },
            };
            _questionService.Setup(x => x.GetFirstQuestion(It.IsAny<string>())).Returns(Task.FromResult(question));
            _questionTransformer.Setup(x => x.AsQuestionWithAnswers(It.IsAny<QuestionWithAnswers>())).Returns(question);

            var result = await _sut.GetFirstQuestion(It.IsAny<string>(), "{\"PATIENT_AGE\":\"24\",\"PATIENT_GENDER\":\"M\"}");
            var node = result.Content;

            Assert.IsTrue(node.State.ContainsKey("SYSTEM_ONLINE"));
            Assert.AreEqual(node.State["SYSTEM_ONLINE"], "online");
            Assert.IsTrue(node.State.ContainsKey("SYSTEM_MERS"));
        }

        [Test]
        public async void set_state_contains_system_variables()
        {
            var set = new QuestionWithAnswers()
            {
                Question = new Question { Title = "SET_STATE" },
                Labels = new[]
                {
                    "Set"
                },
            };
            var question = new QuestionWithAnswers()
            {
                Question = new Question(),
                Labels = new[]
                {
                    "Question"
                },
            };
            var answers = new[] { new Answer { Title = "setstate" } };

            _questionService.Setup(x => x.GetFirstQuestion(It.IsAny<string>())).Returns(Task.FromResult(set));
            _questionService.Setup(x => x.GetAnswersForQuestion(It.IsAny<string>())).Returns(Task.FromResult(answers));
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(question));

            _sut = new QuestionController(_questionService.Object, new QuestionTransformer(), _answersForNodeBuilder.Object, _cacheManager.Object);
            var result = await _sut.GetFirstQuestion(It.IsAny<string>(), "{\"PATIENT_AGE\":\"24\",\"PATIENT_GENDER\":\"M\"}");
            var node = result.Content;

            Assert.IsTrue(node.State.ContainsKey("SET_STATE"));
            Assert.AreEqual(node.State["SET_STATE"], "setstate");
            Assert.IsTrue(node.State.ContainsKey("SYSTEM_ONLINE"));
            Assert.AreEqual(node.State["SYSTEM_ONLINE"], "online");
            Assert.IsTrue(node.State.ContainsKey("SYSTEM_MERS"));
        }

        [Test]
        public async void read_state_contains_system_variables()
        {
            var set = new QuestionWithAnswers()
            {
                Question = new Question { Title = "READ_STATE" },
                Labels = new[]
                {
                    "Read"
                },
            };
            var question = new QuestionWithAnswers()
            {
                Question = new Question(),
                Labels = new[]
                {
                    "Question"
                },
            };
            var answers = new[] { new Answer { Title = "readstate" }, };

            _questionService.Setup(x => x.GetFirstQuestion(It.IsAny<string>())).Returns(Task.FromResult(set));
            _questionService.Setup(x => x.GetAnswersForQuestion(It.IsAny<string>())).Returns(Task.FromResult(answers));
            _questionService.Setup(x => x.GetNextQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(question));
            _answersForNodeBuilder.Setup(x => x.SelectAnswer(It.IsAny<IEnumerable<Answer>>(), It.IsAny<string>())).Returns(new Answer { Title = "readstate" });

            _sut = new QuestionController(_questionService.Object, new QuestionTransformer(), _answersForNodeBuilder.Object, _cacheManager.Object);
            var result = await _sut.GetFirstQuestion(It.IsAny<string>(), "{\"PATIENT_AGE\":\"24\",\"PATIENT_GENDER\":\"M\",\"READ_STATE\":\"readstate\"}");
            var node = result.Content;

            Assert.IsTrue(node.State.ContainsKey("READ_STATE"));
            Assert.AreEqual(node.State["READ_STATE"], "readstate");
            Assert.IsTrue(node.State.ContainsKey("SYSTEM_ONLINE"));
            Assert.AreEqual(node.State["SYSTEM_ONLINE"], "online");
            Assert.IsTrue(node.State.ContainsKey("SYSTEM_MERS"));
        }
    }
}

//TODO put tests in place again
//using NHS111.Business.Services;
//using NHS111.Utils.Helpers;
//using NUnit.Framework;
//using Moq;
//using System.Collections.Generic;
//using System.Threading.Tasks; 

//namespace NHS111.Business.Test.Services
//{
//    [TestFixture]
//    public class QuestionService_Test
//    {
//        private Mock<Configuration.IConfiguration> _configuration;
//        private Mock<IRestfulHelper> _restfulHelper;

//        [SetUp]
//        public void SetUp()
//        {
//             _configuration = new Mock<Configuration.IConfiguration>();
//             _restfulHelper = new Mock<IRestfulHelper>();
//        }   

//        [Test]
//        public async void should_return_a_question_by_id()
//        {
//            //Arrange

//            var url = "http://mytest.com/";
//            var id = "1";
//            var resultString = "is this is a test question?";

//            _configuration.Setup(x => x.GetDomainApiQuestionUrl(id)).Returns(url);
//            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

//            var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

//            //Act
//            var result = await sut.GetQuestion(id);

//            //Assert 
//            _configuration.Verify(x => x.GetDomainApiQuestionUrl(id), Times.Once);
//            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
//            Assert.That(result, Is.EqualTo(resultString));
//        }

//        [Test]
//        public async void should_return_answers_related_to_a_question()
//        {
//            //Arrange

//            var url = "http://mytest.com/";
//            var id = "1";
//            var resultString = "answer1, answer2";

//            _configuration.Setup(x => x.GetDomainApiAnswersForQuestionUrl(id)).Returns(url);
//            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

//            var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

//            //Act
//            var result = await sut.GetAnswersForQuestion(id);

//            //Assert 
//            _configuration.Verify(x => x.GetDomainApiAnswersForQuestionUrl(id), Times.Once);
//            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);

//            Assert.That(result, Is.EqualTo(resultString));
//        }

//        [Test]
//        public async void should_return_following_question()
//        {
//            //Arrange

//            var url = "http://mytest.com/";
//            var id = "idQ1";
//            var answer = "idA1";
//            var resultString = "is this is a test question next?";

//            _configuration.Setup(x => x.GetDomainApiNextQuestionUrl(id, answer)).Returns(url);
//            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));
//            var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

//            //Act
//            var result = await sut.GetNextQuestion(id, answer);

//            //Assert 
//            _configuration.Verify(x => x.GetDomainApiNextQuestionUrl(id, answer), Times.Once);
//            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
//            Assert.That(result, Is.EqualTo(resultString));
//        }

//        [Test]
//        public async void should_return_first_jtbs_question()
//        {
//            //Arrange

//            var url = "http://mytest.com/";
//            var id = "1";
//            var resultString = "is this the first jtbs question?";

//            _configuration.Setup(x => x.GetDomainApiJustToBeSafeQuestionsFirstUrl(id)).Returns(url);
//            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

//            var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

//            //Act
//            var result = await sut.GetJustToBeSafeQuestionsFirst(id);

//            //Assert 
//            _configuration.Verify(x => x.GetDomainApiJustToBeSafeQuestionsFirstUrl(id), Times.Once);
//            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
//            Assert.That(result, Is.EqualTo(resultString));
//        }

//        [Test]
//        public async void should_return_next_jtbs_question()
//        {
//            //Arrange

//            var url = "http://mytest.com/";
//            var pathwayId = "qId1";
//            var resultString = "is this next jtbs question?";

//            var answeredQuestionIds = new List<string> { "1", "2", "3" }; 
//            var multipleChoice = true;
//            var selectedQuestionId = "qId10";

//            _configuration.Setup(x => x.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice,  selectedQuestionId)).Returns(url);
//            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

//            var sut = new QuestionService(_configuration.Object, _restfulHelper.Object);

//            //Act
//            var result = await sut.GetJustToBeSafeQuestionsNext(pathwayId, answeredQuestionIds, multipleChoice,  selectedQuestionId);

//            //Assert 
//            _configuration.Verify(x => x.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice,  selectedQuestionId), Times.Once);
//            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
//            Assert.That(result, Is.EqualTo(resultString));
//        }
//    }
//}
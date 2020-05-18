using Moq;
using NHS111.Domain.Api.Controllers;
using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHS111.Domain.Test.API_Project.Controller
{

    [TestFixture]
    public class QuestionController_Test
    {
        Mock<IQuestionRepository> _questionRepository;
        private QuestionController _sut;

        [SetUp]
        public void SetUp()
        {
            _questionRepository = new Mock<IQuestionRepository>();
            _sut = new QuestionController(_questionRepository.Object);
        }

        [Test]
        public async void should_return_a_question_by_id()
        {
            //Arrange

            var id = "questionid";
            _questionRepository.Setup(x => x.GetQuestion(id)).Returns(Task.FromResult(new QuestionWithAnswers()));

            //Act
            var result = await _sut.GetQuestion(id);

            //Assert
            _questionRepository.Verify(x => x.GetQuestion(id), Times.Once);
            Assert.IsInstanceOf<QuestionWithAnswers>(result.Content);

        }

        [Test]
        public async void should_return_answers_related_to_a_specific_question()
        {
            //Arrange

            IEnumerable<Answer> answerList = new List<Answer>();

            var id = "qId1";
            _questionRepository.Setup(x => x.GetAnswersForQuestion(id)).Returns(Task.FromResult(answerList));


            //Act
            var result = await _sut.GetAnswersForQuestion(id);

            //Assert
            _questionRepository.Verify(x => x.GetAnswersForQuestion(id), Times.Once);
            Assert.IsInstanceOf<IEnumerable<Answer>>(result.Content);
        }

        [Test]
        public async void should_return_next_question()
        {
            //Arrange

            QuestionWithAnswers questionWithAnswers = new QuestionWithAnswers();

            var answer = "answer";
            var id = "qId1";
            var nodeLabel = "Question";
            _questionRepository.Setup(x => x.GetNextQuestion(id, nodeLabel, answer)).Returns(Task.FromResult(questionWithAnswers));


            //Act
            var result = await _sut.GetNextQuestion(id, nodeLabel, answer);

            //Assert
            _questionRepository.Verify(x => x.GetNextQuestion(id, nodeLabel, answer), Times.Once);
            Assert.IsInstanceOf<QuestionWithAnswers>(result.Content);

        }

        [Test]
        public async void should_return_first_just_be_safe_questions()
        {
            //Arrange

            IEnumerable<QuestionWithAnswers> questionWithAnswersList = new List<QuestionWithAnswers>();

            var pathwayId = "qId1";
            var justToBeSafePart = "1";

            _questionRepository.Setup(x => x.GetJustToBeSafeQuestions(pathwayId, justToBeSafePart)).Returns(Task.FromResult(questionWithAnswersList));


            //Act
            var result = await _sut.GetJustToBeSafeQuestionsFirst(pathwayId);

            //Assert
            _questionRepository.Verify(x => x.GetJustToBeSafeQuestions(pathwayId, justToBeSafePart), Times.Once);
            Assert.IsInstanceOf<IEnumerable<QuestionWithAnswers>>(result.Content);

        }


        [Test]
        public async void should_return_next_just_to_be_safe_questions()
        {
            //Arrange
            var pathwayId = "PW755";
            IEnumerable<QuestionWithAnswers> questionWithAnswersList = new List<QuestionWithAnswers>();//{ new QuestionWithAnswers() { Question = new Question(){}, Answers= new List<Answer>(){new Answer()}, Labels = new List<string>(){"label1","label2"}}};
            var answeredQuestionIds = "Q1,Q2";
            var multipleChoice = false;
            var selectedQuestionId = "A1";

            _questionRepository.Setup(x => x.GetJustToBeSafeQuestions(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId)).ReturnsAsync(questionWithAnswersList);


            //Act
            var result = await _sut.GetJustToBeSafeQuestionsNext(pathwayId, selectedQuestionId, multipleChoice, answeredQuestionIds);

            //Assert
            Assert.IsInstanceOf<IEnumerable<QuestionWithAnswers>>(result.Content);
        }
    }
}
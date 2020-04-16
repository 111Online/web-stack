using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Business.Test.Builders
{
    public class QuestionWithAnswersBuilder
    {
        private QuestionWithAnswers _mockQuestionWithAnswers;
        public QuestionWithAnswersBuilder(string questionId, string questionTitle)
        {
            _mockQuestionWithAnswers = new QuestionWithAnswers() { Labels = new List<string>() { "Question" }, Question = new Question() { Id = questionId, QuestionNo = "TX" + questionId, Title = questionTitle } };
        }

        public QuestionWithAnswersBuilder()
        {
            _mockQuestionWithAnswers = new QuestionWithAnswers();
        }

        public QuestionWithAnswersBuilder AddAnswer(string answerText)
        {
            if (_mockQuestionWithAnswers.Answers == null) _mockQuestionWithAnswers.Answers = new List<Answer>();
            _mockQuestionWithAnswers.Answers.Add(new Answer() { Order = _mockQuestionWithAnswers.Answers.Count, Title = answerText });
            return this;
        }

        public QuestionWithAnswers Build()
        {
            return _mockQuestionWithAnswers;
        }
    }
}

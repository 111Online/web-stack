using NHS111.Business.Transformers;
using NHS111.Models.Models.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.Test.Transformers
{
    [TestFixture]
    public class QuestionTransformer_Test
    {
        QuestionTransformer QuestionTransformer;

        [SetUp]
        public void SetUp()
        {
            QuestionTransformer = new QuestionTransformer();
        }


        [Test]
        public void should_return_a_QuestionWithAnswers_list_Answer_Title_Uppercase()
        {
            //Arrange
            List<QuestionWithAnswers> questionWithAnswersList = new List<QuestionWithAnswers>(){

                new QuestionWithAnswers() {
                    Question= new Question(),
                    Answers = new List<Answer>(){
                        new Answer(){
                                Title = "title",
                                SymptomDiscriminator = "SymptomDiscriminatorCode"
                                }
                        },
                    Labels = new List<string>(){"Label1"}
                }

             };

            //Act
            var resultQuestionWithAnswersList = QuestionTransformer.AsQuestionWithAnswersList(questionWithAnswersList);

            //Assert 
            Assert.That(resultQuestionWithAnswersList.ToList()[0].Answers[0].Title, Is.EqualTo("Title"));
        }

        [Test]
        public void should_return_a_QuestionWithAnswer_Answer_Title_Uppercase()
        {

            //Arrange
            QuestionWithAnswers questionWithAnswers =

                new QuestionWithAnswers()
                {
                    Question = new Question(),
                    Answers = new List<Answer>(){
                        new Answer(){
                                Title = "title",
                                SymptomDiscriminator = "SymptomDiscriminatorCode"
                                }
                        },
                    Labels = new List<string>() { "Label1" }
                };

            //Act
            var resultQuestionWithAnswers = QuestionTransformer.AsQuestionWithAnswers(questionWithAnswers);

            //Assert 
            Assert.That(resultQuestionWithAnswers.Answers[0].Title, Is.EqualTo("Title"));

        }

        [Test]
        public void should_return_Answer_Title_Uppercase()
        {

            //Arrange
            List<Answer> answers = new List<Answer>(){
                        new Answer()
                        {
                            Title = "title",
                            SymptomDiscriminator = "SymptomDiscriminatorCode"
                        }};

            //Act
            var resultAnswers = QuestionTransformer.AsAnswers(answers);

            //Assert 
            Assert.That(resultAnswers.ToList()[0].Title, Is.EqualTo("Title"));
        }

        [Test]
        public void should_return_Question_With_Dead_End()
        {

            //Arrange
            QuestionWithAnswers questionWithAnswers =

                new QuestionWithAnswers()
                {
                    Question = new Question(),
                    Labels = new List<string>() { "DeadEndJump" }
                };

            //Act
            var resultQuestionWithDeadEnd = QuestionTransformer.AsQuestionWithDeadEnd(questionWithAnswers);

            //Assert 
            Assert.That(resultQuestionWithDeadEnd.Labels.First(), Is.EqualTo("DeadEndJump"));
        }
    }
}

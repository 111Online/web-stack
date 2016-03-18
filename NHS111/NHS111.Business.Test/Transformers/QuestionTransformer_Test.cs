using NUnit.Framework;
using System.Collections.Generic;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Domain;
using Newtonsoft.Json;

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
                                SymptomDiscriminator = "SymptomDiscriminator"
                                }
                        },
                    Labels = new List<string>(){"Label1"}
                }

             };

            var json = JsonConvert.SerializeObject(questionWithAnswersList);

            //Act
            var result = QuestionTransformer.AsQuestionWithAnswersList(json);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Answers[0].Title, Is.EqualTo("Title"));
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
                                SymptomDiscriminator = "SymptomDiscriminator"
                                }
                        },
                    Labels = new List<string>() { "Label1" }
                };

            var json = JsonConvert.SerializeObject(questionWithAnswers);

            //Act
            var result = QuestionTransformer.AsQuestionWithAnswers(json);
            var resultQuestionWithAnswers = JsonConvert.DeserializeObject<QuestionWithAnswers>(result);

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
                            SymptomDiscriminator = "SymptomDiscriminator"
                        }};

            var json = JsonConvert.SerializeObject(answers);

            //Act
            var result = QuestionTransformer.AsAnswers(json);
            var resultAnswers = JsonConvert.DeserializeObject<List<Answer>>(result);

            //Assert 
            Assert.That(resultAnswers[0].Title, Is.EqualTo("Title"));
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

            var json = JsonConvert.SerializeObject(questionWithAnswers);

            //Act
            var result = QuestionTransformer.AsQuestionWithDeadEnd(json);
            var resultQuestionWithDeadEnd = JsonConvert.DeserializeObject<QuestionWithDeadEnd>(result);

            //Assert 
            Assert.IsInstanceOf<QuestionWithDeadEnd>(resultQuestionWithDeadEnd);
        }
    }
}

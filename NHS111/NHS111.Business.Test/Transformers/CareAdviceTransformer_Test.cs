using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Domain;
using NUnit.Framework;

namespace NHS111.Business.Test.Transformers
{
    [TestFixture]
    public class CareAdviceTransformer_Test
    {
        CareAdviceTransformer CareAdviceTransformer;

        [SetUp]
        public void SetUp()
        {
            CareAdviceTransformer = new CareAdviceTransformer();
        }


        [Test]
        public void should_return_a_QuestionWithAnswers_list_CareAdvice_Pascal_case()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>
            {
                new CareAdvice
                {
                    Id = "CX123456-Adult_Male",
                    Title = "Care Advice 1",
                    Items = new List<CareAdviceText>
                    {
                        new CareAdviceText
                        {
                              Text = "Item 1",
                              OrderNo = 1
                        },
                    }
                }
             };

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Question.CareAdviceId, Is.EqualTo("Cx123456"));
        }

        [Test]
        public void should_return_a_QuestionWithAnswers_list_with_InterimCareAdvice_label()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>
            {
                new CareAdvice
                {
                    Id = "CX123456-Adult_Male",
                    Title = "Care Advice 1",
                    Items = new List<CareAdviceText>
                    {
                        new CareAdviceText
                        {
                            Text = "Item 1",
                            OrderNo = 1
                        },
                    }
                }
            };

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Labels.Contains("InterimCareAdvice"));
        }

        [Test]
        public void should_return_a_QuestionWithAnswers_list_with_answers()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>
            {
                new CareAdvice
                {
                    Id = "CX123456-Adult_Male",
                    Title = "Care Advice 1",
                    Items = new List<CareAdviceText>
                    {
                        new CareAdviceText
                        {
                            Text = "Item 1",
                            OrderNo = 1
                        },
                        new CareAdviceText
                        {
                            Text = "Item 2",
                            OrderNo = 2
                        },
                    }
                }
            };

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Answers.Count == 2);
        }

        [Test]
        public void should_return_a_QuestionWithAnswers_list_with_answer_title()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>
            {
                new CareAdvice
                {
                    Id = "CX123456-Adult_Male",
                    Title = "Care Advice 1",
                    Items = new List<CareAdviceText>
                    {
                        new CareAdviceText
                        {
                            Text = "Item 1",
                            OrderNo = 1
                        },
                        new CareAdviceText
                        {
                            Text = "Item 2",
                            OrderNo = 2
                        },
                    }
                }
            };

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Answers[0].Title == "Item 1");
        }

        [Test]
        public void should_return_a_QuestionWithAnswers_list_with_answer_increments_order()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>
            {
                new CareAdvice
                {
                    Id = "CX123456-Adult_Male",
                    Title = "Care Advice 1",
                    Items = new List<CareAdviceText>
                    {
                        new CareAdviceText
                        {
                            Text = "Item 1",
                            OrderNo = 0
                        },
                        new CareAdviceText
                        {
                            Text = "Item 2",
                            OrderNo = 1
                        },
                    }
                }
            };

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Answers[0].Order == 1);
        }

        [Test]
        public void should_return_a_empty_QuestionWithAnswers_list()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>();

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList.Count == 0);
        }

        [Test]
        public void should_return_a_QuestionWithAnswers_list_with_no_answers()
        {
            //Arrange
            var careAdviceList = new List<CareAdvice>
            {
                new CareAdvice
                {
                    Id = "CX123456-Adult_Male",
                    Title = "Care Advice 1"
                }
            };

            //Act
            var result = CareAdviceTransformer.AsQuestionWithAnswersList(careAdviceList);
            var resultQuestionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(result);

            //Assert 
            Assert.That(resultQuestionWithAnswersList[0].Answers.Count == 0);
        }
    }
}

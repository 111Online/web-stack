using System.Collections.Generic;
using NHS111.Business.Builders;
using NHS111.Models.Models.Domain;
using NUnit.Framework;

namespace NHS111.Business.Test.Builders
{
    [TestFixture]
    public class AnswersForNodeBuilderTests
    {
        [Test]
        public void SelectAnswerLowerCaseValid()
        {
            AnswersForNodeBuilder sut = new AnswersForNodeBuilder();
            Answer a1 = new Answer { Title = "==\"TODDLER\"", Order = 1 };
            Answer a2 = new Answer { Title = "==\"ADULT\"", Order = 2 };
            Answer a3 = new Answer { Title = "==\"INFANT\"", Order = 3 };
            Answer a4 = new Answer { Title = "==\"CHILD\"", Order = 4 };
            Answer a5 = new Answer { Title = "==\"NEONATE\"", Order = 5 };
            Answer a6 = new Answer { Title = "default", Order = 6 };

            List<Answer> answerList = new List<Answer> { a1, a2, a3, a4, a5, a6 };

            var result = sut.SelectAnswer(answerList, "toddler");
            Assert.AreEqual("==\"TODDLER\"", result);
        }

        [Test]
        public void SelectAnswerInvalid()
        {
            AnswersForNodeBuilder sut = new AnswersForNodeBuilder();
            Answer a1 = new Answer { Title = "==\"TODDLER\"", Order = 1 };
            Answer a2 = new Answer { Title = "==\"ADULT\"", Order = 2 };
            Answer a3 = new Answer { Title = "==\"INFANT\"", Order = 3 };
            Answer a4 = new Answer { Title = "==\"CHILD\"", Order = 4 };
            Answer a5 = new Answer { Title = "==\"NEONATE\"", Order = 5 };
            Answer a6 = new Answer { Title = "default", Order = 6 };

            List<Answer> answerList = new List<Answer> { a1, a2, a3, a4, a5, a6 };

            var result = sut.SelectAnswer(answerList, "notananswer");
            Assert.AreEqual("default", result);
        }

        [Test]
        public void SelectPresent()
        {
            AnswersForNodeBuilder sut = new AnswersForNodeBuilder();
            Answer a1 = new Answer { Title = "==\"present\"", Order = 1 };
            Answer a2 = new Answer { Title = "default", Order = 2 };

            List<Answer> answerList = new List<Answer> { a1, a2};

            var result = sut.SelectAnswer(answerList, "\"present\"");
            Assert.AreEqual("==\"present\"", result);
        }
    }
}

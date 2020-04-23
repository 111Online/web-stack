using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class JourneyHistoryWranglerTests
    {
        private readonly IJourneyHistoryWrangler _symptomGroupBuilder = new JourneyHistoryWrangler();

        [Test]
        public void GetPathwayNumbersGroupsSingleQuestion()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            list.Add(js1);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual("PW123", result);
        }

        [Test]
        public void GetPathwayNumbersJumpingBetweenPathways()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = "PW133.100" };
            list.Add(js1);
            list.Add(js2);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual("PW123,PW133", result);
        }

        [Test]
        public void GetPathwayNumbersJumpingBackToPathway()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = "PW133.100" };
            JourneyStep js3 = new JourneyStep { QuestionId = "PW123.330" };
            list.Add(js1);
            list.Add(js2);
            list.Add(js3);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual("PW133,PW123", result);
        }

        [Test]
        public void GetPathwayNumbersJumpingBackToPathwayMultiple()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = "PW143.100" };
            JourneyStep js3 = new JourneyStep { QuestionId = "PW113.100" };
            JourneyStep js4 = new JourneyStep { QuestionId = "PW123.330" };
            list.Add(js1);
            list.Add(js2);
            list.Add(js3);
            list.Add(js4);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual("PW143,PW113,PW123", result);
        }

        [Test]
        public void GetPathwayNumbersEmptyQuestionId()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = string.Empty };
            JourneyStep js3 = new JourneyStep { QuestionId = "PW124.330" };
            list.Add(js1);
            list.Add(js2);
            list.Add(js3);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual("PW123,PW124", result);
        }

        [Test]
        public void GetPathwayNumbersDuplicatePathway()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = "PW123.330" };
            list.Add(js1);
            list.Add(js2);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual("PW123", result);
        }

        [Test]
        public void GetPathwayNumbersEmptyList()
        {
            List<JourneyStep> empty = new List<JourneyStep>();
            var result = _symptomGroupBuilder.GetPathwayNumbers(empty);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void GetPathwayNumbersListWithEmptyItems()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep();
            JourneyStep js2 = new JourneyStep();
            list.Add(js1);
            list.Add(js2);
            //Act
            var result = _symptomGroupBuilder.GetPathwayNumbers(list);
            //Assert
            Assert.AreEqual(0, result.Length);
        }
    }
}

using System.Collections.Generic;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class SymptomGroupBuilderTests
    {
        private readonly ISymptomGroupBuilder _symptomGroupBuilder = new SymptomGroupBuilder();

        [Test]
        public void GetSymptomGroupsSingleQuestion()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            list.Add(js1);
            //Act
            var result = _symptomGroupBuilder.GetSymptomGroups(list);
            //Assert
            Assert.AreEqual("PW123", result);
        }

        [Test]
        public void GetSymptomGroupsJumpingBetweenPathways()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = "PW133.100" };
            list.Add(js1);
            list.Add(js2);
            //Act
            var result = _symptomGroupBuilder.GetSymptomGroups(list);
            //Assert
            Assert.AreEqual("PW123,PW133", result);
        }

        [Test]
        public void GetSymptomGroupsJumpingBackToPathway()
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
            var result = _symptomGroupBuilder.GetSymptomGroups(list);
            //Assert
            Assert.AreEqual("PW133,PW123", result);
        }

        [Test]
        public void GetSymptomGroupsEmptyQuestionId()
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
            var result = _symptomGroupBuilder.GetSymptomGroups(list);
            //Assert
            Assert.AreEqual("PW123,PW124", result);
        }

        [Test]
        public void GetSymptomGroupsDuplicatePathway()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep { QuestionId = "PW123.300" };
            JourneyStep js2 = new JourneyStep { QuestionId = "PW123.330" };
            list.Add(js1);
            list.Add(js2);
            //Act
            var result = _symptomGroupBuilder.GetSymptomGroups(list);
            //Assert
            Assert.AreEqual("PW123", result);
        }

        [Test]
        public void GetSymptomGroupsEmptyList()
        {
            List<JourneyStep> empty = new List<JourneyStep>();
            var result = _symptomGroupBuilder.GetSymptomGroups(empty);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void GetSymptomGroupsListWithEmptyItems()
        {
            //Arrange
            List<JourneyStep> list = new List<JourneyStep>();
            JourneyStep js1 = new JourneyStep();
            JourneyStep js2 = new JourneyStep();
            list.Add(js1);
            list.Add(js2);
            //Act
            var result = _symptomGroupBuilder.GetSymptomGroups(list);
            //Assert
            Assert.AreEqual(0, result.Length);
        }
    }
}

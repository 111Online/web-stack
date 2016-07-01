
namespace NHS111.Models.Test.Models.Web {
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;
    using NUnit.Framework;

    [TestFixture]
    public class JourneyViewModelTests {

        [Test]
        public void StepLink_WithAnswersInJourney_ReturnsCorrectAnswersInLink() {
            var sut = new JourneyViewModel();
            sut.Journey = GenerateRandomJourney();

            var result = sut.StepLink;
            var serialisedAnswers = result.Split('=')[1];
            var answers = serialisedAnswers.Split(',');

            for (int i = 0; i < sut.Journey.Steps.Count; i++) {
                var expectedIndex = sut.Journey.Steps[i].Answer.Order - 1;
                Assert.AreEqual(expectedIndex.ToString(), answers[i]); //tests for presence as well as order
            }
        }

        //will move this to a test helper library at some point
        private Journey GenerateRandomJourney() {
            return new Journey {
                Steps = GenerateRandomSteps()
            };
        }

        private List<JourneyStep> GenerateRandomSteps(int stepCount = -1) {
            if (stepCount < 0) {
                const int MAX_QUESTIONS = 50; //roughly how long could a journey be?
                stepCount = new Random().Next(1, MAX_QUESTIONS);
            }

            var steps = new List<JourneyStep>();
            for (int i = 0; i < stepCount; i++) {
                steps.Add(GenerateRandomStep());
            }
            return steps;
        }

        private JourneyStep GenerateRandomStep() {
            return new JourneyStep {
                Answer = new Answer {
                    Title = "Some answer",
                    Order = new Random().Next()
                }
            };
        }
    }
}

using System.Configuration;
using System.Linq;
using NHS111.Models.Models.Configuration;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Configuration
{
    [TestFixture]
    public class ModZeroJourneysSectionTests
    {
        private const string Section = "moduleZeroJourneys";

        [Test]
        public void ModZeroJourneysSectionCasts()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            Assert.IsNotNull(section);
        }

        [Test]
        public void ModZeroJourneysSectionReturnsCollection()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            Assert.IsNotNull(section.ModuleZeroJourneys);
            Assert.AreEqual(3, section.ModuleZeroJourneys.Count);
        }

        [Test]
        public void ModZeroJourneyElementReturnsCollection()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            var modZeroJourneyElement = section.ModuleZeroJourneys.First();
            Assert.IsNotNull(modZeroJourneyElement.JourneySteps);
            Assert.AreEqual(5, modZeroJourneyElement.JourneySteps.Count);
        }

        [Test]
        public void ModZeroJourneyElementHasCorrectProperties()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            var modZeroJourneyElement = section.ModuleZeroJourneys.First();
            Assert.AreEqual("PA113", modZeroJourneyElement.Id);
            Assert.AreEqual("Adult", modZeroJourneyElement.Age);
            Assert.AreEqual("Female", modZeroJourneyElement.Gender);
            Assert.AreEqual("Trauma", modZeroJourneyElement.Type);
        }

        [Test]
        public void ModZeroJourneyElementReturnCorrectType()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            var modZeroJourneyElement = section.ModuleZeroJourneys.First();
            Assert.IsTrue(modZeroJourneyElement.IsTraumaJourney);
            Assert.IsFalse(section.ModuleZeroJourneys.First(j => j.Id.Equals("PA118")).IsTraumaJourney);
        }

        [Test]
        public void JourneyStepElementHasCorrectProperties()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            var modZeroJourneyElement = section.ModuleZeroJourneys.First();
            var journeyStep = modZeroJourneyElement.JourneySteps.First();
            Assert.AreEqual("1", journeyStep.Id);
            Assert.AreEqual(0, journeyStep.Order);
            Assert.AreEqual(2, modZeroJourneyElement.JourneySteps.First(j => j.Id.Equals("3")).Order);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void GetModZeroJourneyElementReturnsMissingConfigurationException()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            section.ModuleZeroJourneys.GetModZeroJourneyElement("Adult", "Male", true);
        }

        [Test]
        public void GetModZeroJourneyElementReturnsCorrectModZeroJourneyElement()
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            var modZeroJourneyElement = section.ModuleZeroJourneys.GetModZeroJourneyElement("Male", "Child", true);
            Assert.AreEqual("PA125", modZeroJourneyElement.Id);

            modZeroJourneyElement = section.ModuleZeroJourneys.GetModZeroJourneyElement("Male", "Adult", false);
            Assert.AreEqual("PA118", modZeroJourneyElement.Id);

            modZeroJourneyElement = section.ModuleZeroJourneys.GetModZeroJourneyElement("Female", "Adult", true);
            Assert.AreEqual("PA113", modZeroJourneyElement.Id);
        }
    }
}

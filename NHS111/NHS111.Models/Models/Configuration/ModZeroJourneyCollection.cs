using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class ModZeroJourneyCollection : ConfigurationElementCollection
    {
        private const string JourneyStepCollectionName = "journeyStep";

        [ConfigurationProperty(JourneyStepCollectionName)]
        [ConfigurationCollection(typeof(JourneyStepCollection), AddItemName = "journeyStep")]
        public JourneyStepCollection JourneySteps { get { return (JourneyStepCollection)base[JourneyStepCollectionName]; } }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JourneyStepCollection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JourneyStepCollection)element).Id;
        }
    }
}

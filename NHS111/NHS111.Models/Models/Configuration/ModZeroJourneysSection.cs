using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class ModZeroJourneysSection : ConfigurationSection
    {
        private const string ModZeroJourneyCollectionName = "moduleZeroJourneys";

        [ConfigurationProperty(ModZeroJourneyCollectionName)]
        [ConfigurationCollection(typeof(ModZeroJourneyCollection), AddItemName = "moduleZeroJourney")]
        public ModZeroJourneyCollection ModuleZeroJourneys { get { return (ModZeroJourneyCollection)base[ModZeroJourneyCollectionName]; } }
    }
}

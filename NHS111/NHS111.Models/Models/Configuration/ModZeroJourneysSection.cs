using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class ModZeroJourneysSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ModZeroJourneyCollection ModuleZeroJourneys { get { return (ModZeroJourneyCollection)base[""]; } }
    }
}

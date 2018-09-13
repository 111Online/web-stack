using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class ModZeroJourneyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JourneyStepElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JourneyStepElement)element).Id;
        }
    }
}

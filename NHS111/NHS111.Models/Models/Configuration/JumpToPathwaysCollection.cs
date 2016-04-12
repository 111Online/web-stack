using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class JumpToPathwaysCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JumpToPathwaysElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JumpToPathwaysElement)element).Id;
        }
    }
}

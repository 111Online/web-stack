using System.Collections.Generic;
using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class JourneyStepCollection : ConfigurationElementCollection, IEnumerable<JourneyStepElement>
    {
        public JourneyStepElement this[object key]
        {
            get
            {
                return base.BaseGet(key) as JourneyStepElement;
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "journeyStep";
            }
        }

        protected override bool IsElementName(string elementName)
        {
            var isName = false;
            if (!string.IsNullOrEmpty(elementName))
                isName = elementName.Equals("journeyStep");
            return isName;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JourneyStepElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JourneyStepElement)element).Id;
        }

        public new IEnumerator<JourneyStepElement> GetEnumerator()
        {
            var count = base.Count;
            for (var i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as JourneyStepElement;
            }
        }
    }
}


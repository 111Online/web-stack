using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NHS111.Models.Models.Configuration
{
    public class ModZeroJourneyCollection : ConfigurationElementCollection, IEnumerable<ModZeroJourneyElement>
    {
        public ModZeroJourneyElement this[object key]
        {
            get
            {
                return base.BaseGet(key) as ModZeroJourneyElement;
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
                return "moduleZeroJourney";
            }
        }

        protected override bool IsElementName(string elementName)
        {
            var isName = false;
            if (!string.IsNullOrEmpty(elementName))
                isName = elementName.Equals("moduleZeroJourney");
            return isName;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModZeroJourneyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModZeroJourneyElement)element).Id;
        }

        public new IEnumerator<ModZeroJourneyElement> GetEnumerator()
        {
            var count = base.Count;
            for (var i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ModZeroJourneyElement;
            }
        }

        public ModZeroJourneyElement GetModZeroJourneyElement(string gender, string age, bool isTrauma)
        {
            var element = this.FirstOrDefault(j => j.Age == age && j.Gender == gender && j.IsTraumaJourney == isTrauma);
            if (element == null) throw new ConfigurationErrorsException(string.Format("A module zero journey has not been defined for {0} {1} with {2}", gender, age, isTrauma ? "Trauma" : "Non-Trauma"));
            return this.First(j => j.Age == age && j.Gender == gender && j.IsTraumaJourney == isTrauma);
        }
    }
}

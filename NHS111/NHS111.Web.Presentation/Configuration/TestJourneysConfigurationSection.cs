
namespace NHS111.Web.Presentation.Configuration
{
    using System;
    using System.Configuration;
    using System.Xml;

    public class TestJourneysConfigurationSection
        : ConfigurationSection
    {
        [ConfigurationProperty("testJourneys")]
        public TestJourneyElementCollection TestJourneys
        {
            get { return (TestJourneyElementCollection)base["testJourneys"]; }
            set { base["testJourneys"] = value; }
        }
    }

    [ConfigurationCollection(typeof(TestJourneyElement))]
    public class TestJourneyElementCollection
        : ConfigurationElementCollection
    {
        internal const string PropertyName = "testJourney";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TestJourneyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestJourneyElement)(element)).Json;
        }

        public TestJourneyElement this[int idx]
        {
            get { return (TestJourneyElement)BaseGet(idx); }
        }
    }

    public class TestJourneyElement : ConfigurationElement
    {

        [ConfigurationProperty("triggerQuestionNo", IsRequired = false)]
        public string TriggerQuestionNo
        {
            get { return (string)base["triggerQuestionNo"]; }
            set { base["triggerQuestionNo"] = value; }
        }

        [ConfigurationProperty("json", IsKey = true)]
        public string Json
        {
            get { return (string)base["json"]; }
            set { base["json"] = value; }
        }

        protected override void DeserializeElement(XmlReader reader,
            bool serializeCollectionKey)
        {
            TriggerQuestionNo = reader["triggerQuestionNo"];
            Json = reader.ReadElementContentAsString();
        }
    }
}

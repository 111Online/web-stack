using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class JourneyStepCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("age", IsRequired = true)]
        public int Age
        {
            get { return (int)this["age"]; }
            set { this["age"] = value; }
        }

        [ConfigurationProperty("gender", IsRequired = true)]
        public string Gender
        {
            get { return (string)this["gender"]; }
            set { this["gender"] = value; }
        }

        ////[ConfigurationProperty("type", IsRequired = true)]
        ////public string Type
        ////{
        ////    get { return (string)this["type"]; }
        ////    set { this["type"] = value; }
        ////}

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

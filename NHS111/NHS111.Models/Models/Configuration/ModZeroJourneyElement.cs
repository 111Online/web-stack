using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class ModZeroJourneyElement : ConfigurationElement
    {
        private const string JourneyStepCollectionName = "journeySteps";

        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("age", IsRequired = true)]
        public string Age
        {
            get { return (string)this["age"]; }
            set { this["age"] = value; }
        }

        [ConfigurationProperty("gender", IsRequired = true)]
        public string Gender
        {
            get { return (string)this["gender"]; }
            set { this["gender"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("pathwayId", IsRequired = true)]
        public string PathwayId
        {
            get { return (string)this["pathwayId"]; }
            set { this["pathwayId"] = value; }
        }

        [ConfigurationProperty("dispositionId", IsRequired = true)]
        public string DispositionId
        {
            get { return (string)this["dispositionId"]; }
            set { this["dispositionId"] = value; }
        }

        [ConfigurationProperty(JourneyStepCollectionName, IsDefaultCollection = false)]
        public JourneyStepCollection JourneySteps
        {
            get { return (JourneyStepCollection)base[JourneyStepCollectionName]; }
        }

        public bool IsTraumaJourney { get { return Type.Equals("Trauma"); } }
    }
}

using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class JourneyStepElement : ConfigurationElement
    {
        [ConfigurationProperty("answerOrder", IsRequired = true)]
        public int Order
        {
            get { return (int)this["answerOrder"]; }
            set { this["answerOrder"] = value; }
        }

        [ConfigurationProperty("questionId", IsRequired = true)]
        public string Id
        {
            get { return (string)this["answerOrder"]; }
            set { this["answerOrder"] = value; }
        }
    }
}

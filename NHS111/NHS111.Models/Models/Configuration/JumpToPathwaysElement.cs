using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class JumpToPathwaysElement : ConfigurationElement
    {
        [ConfigurationProperty("title", IsRequired = true)]
        public string Title
        {
            get { return (string) this["title"]; }
            set { this["title"] = value; }
        }

        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return (string) this["id"]; }
            set { this["id"] = value; }
        }
    }
}

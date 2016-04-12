using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class PathwaysSection : ConfigurationSection
    {
        public const string SectionName = "PathwaysSection";
        private const string LivePathwaysCollectionName = "livePathways";
        private const string JumpToPathwaysCollectionName = "jumpToPathways";

        [ConfigurationProperty("useLivePathways", DefaultValue = "false", IsRequired = false)]
        public bool UseLivePathways
        {
            get
            {
                return (bool)this["useLivePathways"];
            }
            set
            {
                this["useLivePathways"] = value;
            }
        }

        [ConfigurationProperty(LivePathwaysCollectionName)]
        [ConfigurationCollection(typeof(LivePathwaysCollection), AddItemName = "add")]
        public LivePathwaysCollection LivePathways { get { return (LivePathwaysCollection)base[LivePathwaysCollectionName]; } }

        [ConfigurationProperty(JumpToPathwaysCollectionName)]
        [ConfigurationCollection(typeof(JumpToPathwaysCollection), AddItemName = "add")]
        public JumpToPathwaysCollection JumpToPathways { get { return (JumpToPathwaysCollection)base[JumpToPathwaysCollectionName]; } }
    }
}

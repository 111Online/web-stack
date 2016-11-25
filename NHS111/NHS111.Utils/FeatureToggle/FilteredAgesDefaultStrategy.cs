using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using NHS111.Models.Models.Domain;

namespace NHS111.Utils.FeatureToggle
{
    public class FilteredAgesDefaultStrategy : IDefaultSettingStrategy<string>
    {
        public string GetDefaultSetting()
        {
            // "infant|toddler"
            return string.Empty;
        }
    }
}

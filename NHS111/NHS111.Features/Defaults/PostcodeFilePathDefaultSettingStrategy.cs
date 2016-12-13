using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Features.Defaults
{
    public class PostcodeFilePathDefaultSettingStrategy : IDefaultSettingStrategy
    {
        public string Value { get { return @"postcodes.csv"; } }
    }
}

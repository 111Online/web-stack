using NHS111.Features.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Features
{
    public interface IEmailCollectionFeature : IFeature { }

    class EmailCollectionFeature : BaseFeature, IEmailCollectionFeature
    {
        public EmailCollectionFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }
}

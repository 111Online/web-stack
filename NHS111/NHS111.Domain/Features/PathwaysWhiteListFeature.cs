using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Utils;
using NHS111.Utils.FeatureToggle;

namespace NHS111.Domain.Api.Features
{
    public class PathwaysWhiteListFeature : BaseFeature, IPathwaysWhiteListFeature
    {

        public PathwaysWhiteListFeature()
        {
            DefaultSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IPathwaysWhiteListFeature
        : IFeature { }
    
}


using NHS111.Features.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Features
{
    public class FilterServicesFeature : BaseFeature, IFilterServicesFeature
    {

        public FilterServicesFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }

    public interface IFilterServicesFeature : IFeature
    { }
}

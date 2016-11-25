using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Utils.FeatureToggle;

namespace NHS111.Web.Presentation.Features
{
    public class DOSFilteringToggleFeature : BaseFeature, IDOSFilteringToggleFeature {

        public DOSFilteringToggleFeature() {
            DefaultBoolSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IDOSFilteringToggleFeature
        : IFeature { }

}

using System.Collections.Generic;
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class FilterPathwaysByAgeFeature : BaseFeature, IFilterPathwaysByAgeFeature
    {

        public FilterPathwaysByAgeFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }

        public IEnumerable<string> FilteredAgeCategories
        {
            get
            {
                var x = FeatureValue(new FilteredAgesDefaultStrategy(), "AgeCategories");
                return (!string.IsNullOrEmpty(x.Value)) ? x.Value.Split('|') : new string[0];
            }
        }
    }

    public interface IFilterPathwaysByAgeFeature : IFeature
    {
        IEnumerable<string> FilteredAgeCategories { get; }
    }
}

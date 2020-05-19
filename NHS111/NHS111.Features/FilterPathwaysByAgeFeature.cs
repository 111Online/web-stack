using NHS111.Features.Defaults;
using System.Collections.Generic;

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
                var ageCategories = FeatureValue(new FilteredAgesDefaultStrategy(), "AgeCategories");
                return (!string.IsNullOrEmpty(ageCategories.Value)) ? ageCategories.Value.Split('|') : new string[0];
            }
        }
    }

    public interface IFilterPathwaysByAgeFeature : IFeature
    {
        IEnumerable<string> FilteredAgeCategories { get; }
    }
}

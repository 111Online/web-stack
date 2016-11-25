using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Utils.FeatureToggle;

namespace NHS111.Web.Presentation.Features
{
    public class FilterPathwaysByAgeFeature : BaseFeature, IFilterPathwaysByAgeFeature
    {

        public FilterPathwaysByAgeFeature()
        {
            DefaultBoolSettingStrategy = new EnabledByDefaultSettingStrategy();
            DefaultStringSettingStrategy = new FilteredAgesDefaultStrategy();
        }

        public IEnumerable<AgeCategory> FilteredAgeCategories
        {
            get { return (!string.IsNullOrEmpty(StringValue)) ? StringValue.Split('|').Select(a => new AgeCategory(a)) : new List<AgeCategory>(); }
        }
    }

    public interface IFilterPathwaysByAgeFeature : IFeature
    {
        IEnumerable<AgeCategory> FilteredAgeCategories { get; }
    }
}

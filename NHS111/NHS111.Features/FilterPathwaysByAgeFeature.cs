using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class FilterPathwaysByAgeFeature : BaseFeature, IFilterPathwaysByAgeFeature
    {

        public FilterPathwaysByAgeFeature()
        {
            DefaultBoolSettingStrategy = new EnabledByDefaultSettingStrategy();
            DefaultStringSettingStrategy = new FilteredAgesDefaultStrategy();
        }

        //public IEnumerable<AgeCategory> FilteredAgeCategories
        //{
        //    get { return (!string.IsNullOrEmpty(StringValue)) ? StringValue.Split('|').Select(a => new AgeCategory(a)) : new List<AgeCategory>(); }
        //}
    }

    public interface IFilterPathwaysByAgeFeature : IFeature
    {
        //IEnumerable<AgeCategory> FilteredAgeCategories { get; }
    }
}

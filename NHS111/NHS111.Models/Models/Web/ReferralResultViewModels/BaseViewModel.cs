namespace NHS111.Models.Models.Web
{
    public abstract class BaseViewModel
    {
        public abstract string PageTitle { get; }
        public AnalyticsDataLayerContainer AnalyticsDataLayer { get; set; }
    }
}

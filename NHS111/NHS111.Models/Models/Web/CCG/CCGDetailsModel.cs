namespace NHS111.Models.Models.Web.CCG
{
    public class CCGDetailsModel :CCGModel
    {
        public string STPName { get; set; }

        public ServiceListModel ServiceIdWhitelist { get; set; }

        public ServiceListModel ITKServiceIdWhitelist { get; set; }
    }
}

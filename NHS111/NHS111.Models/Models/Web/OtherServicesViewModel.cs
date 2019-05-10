using System.Collections.Generic;

namespace NHS111.Models.Models.Web
{
    public class OtherServicesViewModel : OutcomeViewModel
    {
        public IEnumerable<RecommendedServiceViewModel> OtherServices { get; set; }

        public OtherServicesViewModel() : this(new List<RecommendedServiceViewModel>())
        {
        }

        public OtherServicesViewModel(IEnumerable<RecommendedServiceViewModel> otherServices)
        {
            OtherServices = otherServices;
        }
    }
}

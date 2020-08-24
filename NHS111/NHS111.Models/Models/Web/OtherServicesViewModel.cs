using System.Collections.Generic;
using System.Linq;

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

    public static class RecommendedServiceViewModelExtensions
    {
        public static bool OnlyServiceIsPharmacyCAS(this IEnumerable<RecommendedServiceViewModel> recommendedServices)
        {
            return recommendedServices.Count() == 1 && recommendedServices.First().IsPharmacyCASCallback();
        }
    }
}

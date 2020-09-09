using NHS111.Models.Models.Web.FromExternalServices;
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
        public enum ServiceTypeOffered
        {
            None,
            PharmaCAS,
            ITK,
            Both
        }

        public static ServiceTypeOffered GetServiceTypeOffered(this IEnumerable<RecommendedServiceViewModel> recommendedServices)
        {
            if (ServiceListContainsNonPharmaCASITKService(recommendedServices) && ServiceListContainsPharmaCASService(recommendedServices))
                return ServiceTypeOffered.Both;

            if (ServiceListContainsPharmaCASService(recommendedServices))
                return ServiceTypeOffered.PharmaCAS;

            if (ServiceListContainsNonPharmaCASITKService(recommendedServices))
                return ServiceTypeOffered.ITK;
            
            return ServiceTypeOffered.None;
        }

        private static bool ServiceListContainsPharmaCASService(IEnumerable<RecommendedServiceViewModel> recommendedServices)
        {
            return recommendedServices.Any(s => s.ServiceType.Id == 138);
        }

        private static bool ServiceListContainsNonPharmaCASITKService(IEnumerable<RecommendedServiceViewModel> recommendedServices)
        {
            return recommendedServices.Any(s => s.ServiceType.Id != 138 && (s.OnlineDOSServiceType == OnlineDOSServiceType.Callback || s.OnlineDOSServiceType == OnlineDOSServiceType.ReferRingAndGo));
        }

        public static bool OnlyServiceIsPharmacyCAS(this IEnumerable<RecommendedServiceViewModel> recommendedServices)
        {
            return recommendedServices.Count() == 1 && recommendedServices.First().IsPharmacyCASCallback();
        }
    }
}

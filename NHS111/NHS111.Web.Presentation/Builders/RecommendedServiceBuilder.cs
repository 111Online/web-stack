using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Web.Presentation.Builders
{
    public class RecommendedServiceBuilder : BaseBuilder, IRecommendedServiceBuilder
    {
        public async Task<RecommendedServiceViewModel> BuildRecommendedService(ServiceViewModel firstService)
        {
            //only service using recommended service view is pharmacy currently
            //will need to implement a proper solution to custom content if/when we introduce other
            //service types - maybe a new type/properties in neo4j?
            var recommendedService = Mapper.Map<RecommendedServiceViewModel>(firstService);
            if (recommendedService == null) return await Task.FromResult((RecommendedServiceViewModel) null);

            recommendedService.ReasonText = "This service needs some more details from you. To use it, fill in the form and they’ll be in touch.";
            recommendedService.Details = new DetailsViewModel
            {
                Summary = "Why should I give my details?",
                Text = "If we send your details to the pharmacy they won’t charge more than standard NHS prices for any medicine you’re given.You won’t pay if you’re exempt from prescription charges."
            };
            return await Task.FromResult(recommendedService);
        }

        public async Task<IEnumerable<RecommendedServiceViewModel>> BuildRecommendedServicesList(IEnumerable<ServiceViewModel> services)
        {
            var recommendedServices = new List<RecommendedServiceViewModel>();
            foreach (var service in services)
            {
                var recommendedService = await BuildRecommendedService(service);
                recommendedServices.Add(recommendedService);
            }

            return recommendedServices;
        }
    }
    public interface IRecommendedServiceBuilder
    {
        Task<RecommendedServiceViewModel> BuildRecommendedService(ServiceViewModel firstService);
        Task<IEnumerable<RecommendedServiceViewModel>> BuildRecommendedServicesList(IEnumerable<ServiceViewModel> services);
    }
}

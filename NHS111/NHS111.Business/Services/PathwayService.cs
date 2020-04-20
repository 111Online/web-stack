using System.Collections.Generic;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Business.Caching;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.Services
{
    public class PathwayService : IPathwayService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
        private readonly ICacheStore _cacheStore;

        public PathwayService(IConfiguration configuration, IRestClient restClientDomainApi, ICacheStore cacheStore)
        {
            _configuration = configuration;
            _restClient = restClientDomainApi;
            _cacheStore = cacheStore;
        }

        public async Task<IEnumerable<Pathway>>GetPathways(bool grouped, bool startingOnly)
        {
            var pathways = await _restClient.ExecuteTaskAsync<IEnumerable<Pathway>>(new JsonRestRequest(_configuration.GetDomainApiPathwaysUrl(grouped, startingOnly), Method.GET));
            return pathways.Data;
        }

        public async Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool grouped, bool startingOnly)
        {
            return await _cacheStore.GetOrAdd(new GroupedPathwaysCacheKey(grouped, startingOnly), async () => 
            {
                var pathways = await _restClient.ExecuteTaskAsync<IEnumerable<GroupedPathways>>(new JsonRestRequest(_configuration.GetDomainApiGroupedPathwaysUrl(grouped, startingOnly), Method.GET));
                return pathways.Data;
            });  
        }

        public async Task<IEnumerable<Pathway>> GetPathways(bool grouped, bool startingOnly, string gender, int age)
        {
            return await _cacheStore.GetOrAdd(new PathwaysCacheKey(grouped, startingOnly, gender, age), async () =>
            {
                var pathways = await _restClient.ExecuteTaskAsync<IEnumerable<Pathway>>(
                    new JsonRestRequest(_configuration.GetDomainApiPathwaysUrl(grouped, startingOnly, gender, age),
                        Method.GET));
                return pathways.Data;
            });
        }

        public async Task<Pathway> GetPathway(string pathwayId)
        {
           return await _cacheStore.GetOrAdd(new PathwayCacheKey(pathwayId), async () =>
            {
                var pathways =
                    await _restClient.ExecuteTaskAsync<Pathway>(
                        new JsonRestRequest(_configuration.GetDomainApiPathwayUrl(pathwayId), Method.GET));
                return pathways.Data;
            });

        }

        public async Task<PathwayMetaData> GetPathwayMetaData(string pathwayId)
        {
            return await _cacheStore.GetOrAdd(new PathwayMetaDataCacheKey(pathwayId), async () =>
            {
                var pathways = await _restClient.ExecuteTaskAsync<PathwayMetaData>(
                    new JsonRestRequest(_configuration.GetDomainApiPathwayMetadataUrl(pathwayId), Method.GET));
                return pathways.Data;
            });

        }

        public async Task<string> GetSymptomGroup(string pathwayNumbers)
        {
            var pathways = await _restClient.ExecuteTaskAsync<string>(new JsonRestRequest(_configuration.GetDomainApiPathwaySymptomGroup(pathwayNumbers), Method.GET));
            return pathways.Data;
        }

        public async Task<Pathway> GetIdentifiedPathway(string pathwayNumbers, string gender, int age)
        {
            var pathways = await _restClient.ExecuteTaskAsync<Pathway>(new JsonRestRequest(_configuration.GetDomainApiIdentifiedPathwayUrl(pathwayNumbers, gender, age), Method.GET));
            return pathways.Data;
        }

        public async Task<Pathway> GetIdentifiedPathwayFromTitle(string pathwayTitle, string gender, int age)
        {
            var pathways = await _restClient.ExecuteTaskAsync<Pathway>(new JsonRestRequest(_configuration.GetDomainApiIdentifiedPathwayFromTitleUrl(pathwayTitle, gender, age), Method.GET));
            return pathways.Data;
        }

        public async Task<string> GetPathwayNumbers(string pathwayTitle)
        {
            var pathways = await _restClient.ExecuteTaskAsync<string>(new JsonRestRequest(_configuration.GetDomainApiPathwayNumbersUrl(pathwayTitle), Method.GET));
            return pathways.Data;
        }
    }

    public interface IPathwayService
    {
        Task<IEnumerable<Pathway>> GetPathways(bool grouped, bool startingOnly);
        Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool grouped, bool startingOnly);
        Task<IEnumerable<Pathway>> GetPathways(bool grouped, bool startingOnly, string gender, int age);
        Task<Pathway> GetPathway(string pathwayId);
        Task<PathwayMetaData> GetPathwayMetaData(string pathwayId);
        Task<string> GetSymptomGroup(string pathwayNumbers);
        Task<Pathway> GetIdentifiedPathway(string pathwayNumbers, string gender, int age);
        Task<Pathway> GetIdentifiedPathwayFromTitle(string pathwayTitle, string gender, int age);
        Task<string> GetPathwayNumbers(string pathwayTitle);
    }
}
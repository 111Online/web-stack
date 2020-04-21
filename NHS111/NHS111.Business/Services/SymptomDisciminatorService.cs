using System;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Business.Caching;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.Services
{

    public class SymptomDisciminatorService : ISymptomDisciminatorService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
        private readonly ICacheStore _cacheStore;

        public SymptomDisciminatorService(IConfiguration configuration, IRestClient restClientDomainApi, ICacheStore cacheStore)
        {
            _configuration = configuration;
            _restClient = restClientDomainApi;
            _cacheStore = cacheStore;
        }

        public async Task<SymptomDiscriminator> GetSymptomDisciminator(string id)
        {
            return await _cacheStore.GetOrAdd(new SymptomDiscriminatorCacheKey(id), async () =>
            {
                var symptomDiscriminators = await _restClient.ExecuteTaskAsync<SymptomDiscriminator>(
                    new JsonRestRequest(_configuration.GetDomainApiSymptomDisciminatorUrl(id), Method.GET));
                if (!symptomDiscriminators.IsSuccessful)
                    throw new Exception(string.Format("A problem occured requesting {0}. {1}",
                        _configuration.GetDomainApiSymptomDisciminatorUrl(id), symptomDiscriminators.ErrorMessage));
                return symptomDiscriminators.Data;
            });

        }

    }

    public interface ISymptomDisciminatorService
    {
        Task<SymptomDiscriminator> GetSymptomDisciminator(string id);
    }
}

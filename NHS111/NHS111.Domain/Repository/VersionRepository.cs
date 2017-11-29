using System.Threading.Tasks;
using Neo4jClient.Cypher;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Repository
{
    public class VersionRepository : IVersionRepository
    {
        public VersionRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<VersionInfo> GetInfo()
        {
            return await _graphRepository.Client.Cypher
                .Match("(v:Version)")
                .Return(v => Return.As<VersionInfo>("v"))
                .ResultsAsync
                .FirstOrDefault();
        }

        private readonly IGraphRepository _graphRepository;
    }

    public interface IVersionRepository
    {
        Task<VersionInfo> GetInfo();
    }
}

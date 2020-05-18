using Neo4jClient.Cypher;
using NHS111.Utils.Extensions;
using System.Threading.Tasks;

namespace NHS111.Domain.Repository
{
    public class MonitorRepository : IMonitorRepository
    {
        private readonly IGraphRepository _graphRepository;

        public MonitorRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<bool> CheckHealth()
        {
            var result = await _graphRepository.Client.Cypher.
                Match("(p:Pathway)").
                Return(p => Return.As<int>("count(p)")).
                ResultsAsync.FirstOrDefault();

            return result > 0;
        }
    }

    public interface IMonitorRepository
    {
        Task<bool> CheckHealth();
    }
}

namespace NHS111.Domain.Repository {

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Models.Domain;

    public class OutcomeRepository
        : IOutcomeRepository {

        public OutcomeRepository(IGraphRepository graphRepository) {
            _graphRepository = graphRepository;
        }

        public async Task<IEnumerable<Outcome>> ListOutcomes() {
            var outcomeNodeName = "Outcome";

            return await _graphRepository.Client.Cypher.
                Match(string.Format("(n:{0})", outcomeNodeName)).
                Return(n => n.As<Outcome>()).
                ResultsAsync;
        }

        private readonly IGraphRepository _graphRepository;
    }

    public interface IOutcomeRepository {
        Task<IEnumerable<Outcome>> ListOutcomes();
    }
}
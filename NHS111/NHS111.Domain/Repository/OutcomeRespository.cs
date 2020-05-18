
namespace NHS111.Domain.Repository
{

    using Models.Models.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OutcomeRepository
        : IOutcomeRepository
    {

        public OutcomeRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<IEnumerable<Outcome>> ListOutcomes()
        {
            var outcomeNodeName = "Outcome";

            return await _graphRepository.Client.Cypher.
                Match(string.Format("(n:{0})", outcomeNodeName)).
                Return(n => n.As<Outcome>()).
                ResultsAsync;
        }

        private readonly IGraphRepository _graphRepository;
    }

    public interface IOutcomeRepository
    {
        Task<IEnumerable<Outcome>> ListOutcomes();
    }
}
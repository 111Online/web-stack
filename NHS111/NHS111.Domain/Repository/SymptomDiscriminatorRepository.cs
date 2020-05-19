namespace NHS111.Domain.Repository
{
    using Models.Models.Domain;
    using System.Threading.Tasks;
    using Utils.Extensions;

    public class SymptomDiscriminatorRepository
        : ISymptomDiscriminatorRepository
    {

        public SymptomDiscriminatorRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<SymptomDiscriminator> Get(int id)
        {

            var matchText = string.Format("(s:{0} {{ {1}: \"{2}\" }})", SymptomDiscriminatorNodeName, IdFieldName, id);

            return await _graphRepository.Client.Cypher.
                Match(matchText).
                Return(s => s.As<SymptomDiscriminator>()).
                ResultsAsync.
                FirstOrDefault();
        }

        public const string SymptomDiscriminatorNodeName = "SymptomDiscriminator";

        public const string IdFieldName = "id";

        private readonly IGraphRepository _graphRepository;
    }

    public interface ISymptomDiscriminatorRepository
    {
        Task<SymptomDiscriminator> Get(int id);
    }
}
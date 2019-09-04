using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using NHS111.Domain.Configuration;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Repository
{
    public class ServiceDefinitionRepository : IServiceDefinitionRepository
    {
        private readonly IGraphRepository _graphRepository;
        private readonly IConfiguration _configuration;

        public ServiceDefinitionRepository(IGraphRepository graphRepository, IConfiguration configuration)
        {
            _graphRepository = graphRepository;
            _configuration = configuration;
        }

        public async Task<ServiceDefinition> GetServiceDefinition(string pathwayNo)
        {
            var version = await _graphRepository.Client.Cypher
                .Match("(v:Version)")
                .Return<VersionInfo>("v")
                .ResultsAsync
                .FirstOrDefault();

            var pathways = await _graphRepository.Client.Cypher
                .Match(string.Format("(p:Pathway {{ id: \"{0}\" }})", pathwayNo))
                .ReturnDistinct<Pathway>("p")
                .ResultsAsync;

            var questionsAndAnswers = await _graphRepository.Client.Cypher
                .Match("()-[a]->(q)")
                .Where(string.Format("q.id =~ '.*{0}.*' AND (q:Question OR q:Set)", pathwayNo))
                .With("COLLECT({question:q, answer:a}) AS QuestionsWithPrevAnswer")
                .Unwind("QuestionsWithPrevAnswer", "rows")
                .With("rows.question as question, rows.answer as answer")
                .ReturnDistinct(question => new QuestionWithAnswers()
                {
                    Question = Return.As<Question>("question"),
                    Answered = Return.As<Answer>("answer"),
                    Labels = question.Labels()
                })
                .ResultsAsync;

            var pathway = pathways.First();
            return new ServiceDefinition
            {
                Title = pathway.Title,
                Version = _configuration.GetPathwaysVersion(),
                EffectiveDate = Convert.ToDateTime(version.Date),
                Id = pathway.PathwayNo,
                Pathways = pathways,
                QuestionsAndSets = questionsAndAnswers
            };
        }
    }

    public interface IServiceDefinitionRepository
    {
        Task<ServiceDefinition> GetServiceDefinition(string pathwayNo);
    }
}

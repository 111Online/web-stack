using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Configuration;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Repository
{
    public class PathwayRepository : IPathwayRepository
    {
        private readonly IGraphRepository _graphRepository;
        private readonly IPathwaysConfigurationManager _pathwaysConfigurationManager;

        public PathwayRepository(IGraphRepository graphRepository, IPathwaysConfigurationManager pathwaysConfigurationManager)
        {
            _graphRepository = graphRepository;
            _pathwaysConfigurationManager = pathwaysConfigurationManager;
        }

        public async Task<Pathway> GetPathway(string id)
        {
            return await _graphRepository.Client.Cypher
                .Match(_pathwaysConfigurationManager.UseLivePathways, string.Format("(p:Pathway {{ id: \"{0}\" }})", id))
                .Return(p => p.As<Pathway>())
                .ResultsAsync
                .FirstOrDefault();
        }

        public async Task<Pathway> GetIdentifiedPathway(IEnumerable<string> pathwayNumbers, string gender, int age)
        {
            var genderIs = new Func<string, string>(g => string.Format("(p.gender is null or p.gender = \"\" or p.gender = \"{0}\")", g));
            var ageIsAboveMinimum = new Func<int, string>(a => string.Format("(p.minimumAgeInclusive is null or p.minimumAgeInclusive = \"\" or p.minimumAgeInclusive <= {0})", a));
            var ageIsBelowMaximum = new Func<int, string>(a => string.Format("(p.maximumAgeExclusive is null or p.maximumAgeExclusive = \"\" or {0} < p.maximumAgeExclusive)", a));
            var pathwayNumberIn = new Func<IEnumerable<string>, string>(p => string.Format("p.pathwayNo in {0}", JsonConvert.SerializeObject(p)));

            return await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Join(" and ", new List<string> { genderIs(gender), ageIsAboveMinimum(age), ageIsBelowMaximum(age), pathwayNumberIn(pathwayNumbers) }), _pathwaysConfigurationManager.UseLivePathways)
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync
                .FirstOrDefault();
        }

        public async Task<IEnumerable<Pathway>> GetAllPathways()
        {
            return await _graphRepository.Client.Cypher
                .Match(_pathwaysConfigurationManager.UseLivePathways, "(p:Pathway)")
                .Return(p => p.As<Pathway>())
                .ResultsAsync;
        }

        public async Task<IEnumerable<GroupedPathways>> GetGroupedPathways()
        {
            //match (p:Pathway) return DISTINCT(p.group), collect (p.pathwayNo)
            return await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where("p.module = \"1\"", _pathwaysConfigurationManager.UseLivePathways)
                .Return(p => new GroupedPathways { Group = Return.As<string>("distinct(p.title)"), PathwayNumbers = Return.As<IEnumerable<string>>("collect(p.pathwayNo)") })
                .ResultsAsync;
        }

        public async Task<string> GetSymptomGroup(IList<string> pathwayNos)
        {
            var symptomGroups = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Format("p.pathwayNo in [{0}]", string.Join(", ", pathwayNos.Select(p => "\"" + p + "\""))), _pathwaysConfigurationManager.UseLivePathways)
                .Return(p => new SymptomGroup { PathwayNo = Return.As<string>("p.pathwayNo"), Code = Return.As<string>("collect(distinct(p.symptomGroup))[0]")})
                .ResultsAsync;

            var symptomGroup = symptomGroups
                .OrderBy(group => pathwayNos.IndexOf(group.PathwayNo))
                .LastOrDefault(group => !string.IsNullOrEmpty(group.Code));

            return symptomGroup == null ? string.Empty : symptomGroup.Code;
        }
    }

    public interface IPathwayRepository
    {
        Task<Pathway> GetPathway(string id);
        Task<Pathway> GetIdentifiedPathway(IEnumerable<string> pathwayNumbers, string gender, int age);
        Task<IEnumerable<Pathway>> GetAllPathways();
        Task<IEnumerable<GroupedPathways>> GetGroupedPathways();
        Task<string> GetSymptomGroup(IList<string> pathwayNos);
    }

    
}
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using Newtonsoft.Json;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;
using NHS111.Utils.Parser;

namespace NHS111.Domain.Repository
{
    public class PathwayRepository : IPathwayRepository
    {
        private readonly IGraphRepository _graphRepository;
        
        public PathwayRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<Pathway> GetPathway(string id)
        {
            return await _graphRepository.Client.Cypher
                .Match(string.Format("(p:Pathway {{ id: \"{0}\" }})", id))
                .Return(p => p.As<Pathway>())
                .ResultsAsync
                .FirstOrDefault();
        }

        public async Task<PathwayMetaData> GetPathwayMetadata(string id)
        {
            return await _graphRepository.Client.Cypher
                .Match(string.Format("(p:PathwayMetaData {{ pathwayNo: \"{0}\" }})", id))
                .Return(p => p.As<PathwayMetaData>())
                .ResultsAsync
                .FirstOrDefault();
        }

        public async Task<Pathway> GetIdentifiedPathway(IEnumerable<string> pathwayNumbers, string gender, int age)
        {
            var genderIs = new Func<string, string>(g => string.Format("(p.gender is null or p.gender = \"\" or p.gender = \"{0}\")", g));
            var ageIsAboveMinimum = new Func<int, string>(a => string.Format("(p.minimumAgeInclusive is null or p.minimumAgeInclusive = \"\" or p.minimumAgeInclusive <= {0})", a));
            var ageIsBelowMaximum = new Func<int, string>(a => string.Format("(p.maximumAgeExclusive is null or p.maximumAgeExclusive = \"\" or {0} < p.maximumAgeExclusive)", a));
            var pathwayNumberIn = new Func<IEnumerable<string>, string>(p => string.Format("p.pathwayNo in {0}", JsonConvert.SerializeObject(p)));

            var pathway = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Join(" and ", new List<string> { genderIs(gender), ageIsAboveMinimum(age), ageIsBelowMaximum(age), pathwayNumberIn(pathwayNumbers) }))
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync
                .FirstOrDefault();

            return pathway;
        }

        public async Task<Pathway> GetIdentifiedPathway(string pathwayTitle, string gender, int age)
        {
            var pathwayTitleEquals = string.Format("p.title = {0}", JsonConvert.SerializeObject(pathwayTitle));

            var pathway = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("p", gender), FilterStatements.AgeIsAboveMinimum("p", age), FilterStatements.AgeIsBelowMaximum("p", age), pathwayTitleEquals }))
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync
                .FirstOrDefault();

            return pathway;
        }

        public async Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly)
        {
            return await GetPathwayQuery(startingOnly)
                .Return(p => Return.As<Pathway>("p")).ResultsAsync;
        }

        public async Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly, string gender, int age)
        {
            return await GetPathwayQuery(startingOnly)
                .Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("p", gender), FilterStatements.AgeIsAboveMinimum("p", age), FilterStatements.AgeIsBelowMaximum("p", age) }))
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync;
        }

        public async Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly)
        {
            var query = GetPathwayQuery(startingOnly)
                .Return(p => new GroupedPathways { Group = Return.As<string>("distinct(m.digitalDescription)"), PathwayNumbers = Return.As<IEnumerable<string>>("collect(distinct(m.pathwayNo))") });

            return await query.ResultsAsync;
        }

        public async Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly, string gender, int age)
        {
            var query = GetPathwayQuery(startingOnly)
                .Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("p", gender), FilterStatements.AgeIsAboveMinimum("p", age), FilterStatements.AgeIsBelowMaximum("p", age) }))
                .Return(p => new GroupedPathways { Group = Return.As<string>("distinct(m.digitalDescription)"), PathwayNumbers = Return.As<IEnumerable<string>>("collect(distinct(m.pathwayNo))") });

            return await query.ResultsAsync;
        }

        private ICypherFluentQuery GetPathwayQuery(bool startingOnly)
        {
            var pathwayQuery = _graphRepository.Client.Cypher
               .Match("(p:Pathway)-[:isDescribedAs]->(m:PathwayMetaData)");

            return startingOnly ? pathwayQuery.Where("p.startingPathway = true") : pathwayQuery;
        }

        public async Task<string> GetSymptomGroup(IList<string> pathwayNos)
        {
            var symptomGroups = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Format("p.pathwayNo in [{0}]", string.Join(", ", pathwayNos.Select(p => "\"" + p + "\""))))
                .Return(p => new SymptomGroup { PathwayNo = Return.As<string>("p.pathwayNo"), Code = Return.As<string>("collect(distinct(p.symptomGroup))[0]")})
                .ResultsAsync;

            var symptomGroup = symptomGroups
                .OrderBy(group => pathwayNos.IndexOf(group.PathwayNo))
                .LastOrDefault(group => !string.IsNullOrEmpty(group.Code));

            return symptomGroup == null ? string.Empty : symptomGroup.Code;
        }

        public async Task<string> GetPathwaysNumbers(string pathwayTitle)
        {
            var pathwayNumberList = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Format("p.title =~ \"(?i){0}\"", PathwayTitleUriParser.EscapeSymbols(pathwayTitle)))  //case-insensitive query
                .Return(p => Return.As<string>("p.pathwayNo"))
                .ResultsAsync;

            var pathwayNumbers = pathwayNumberList as string[] ?? pathwayNumberList.ToArray();
            return !pathwayNumbers.Any() ? string.Empty : string.Join(",", pathwayNumbers.Distinct());
        }

    }

    public static class FilterStatements
    {
        public static string GenderIs(string pathwayVariableIdent, string gender)
        {
            return String.Format("({0}.gender is null or {0}.gender = \"\" or {0}.gender = \"{1}\")", pathwayVariableIdent, gender);
        }


        public static string AgeIsAboveMinimum(string pathwayVariableIdent, int age)
        {
            return
                String.Format(
                    "({0}.minimumAgeInclusive is null or {0}.minimumAgeInclusive = \"\" or {0}.minimumAgeInclusive <= {1})",
                    pathwayVariableIdent, age);
        }

        public static string AgeIsBelowMaximum(string pathwayVariableIdent, int age)
        {
            return
                String.Format(
                    "({0}.maximumAgeExclusive is null or {0}.maximumAgeExclusive = \"\" or {1} < {0}.maximumAgeExclusive)", pathwayVariableIdent, age);
        }
    }

    public interface IPathwayRepository
    {
        Task<Pathway> GetPathway(string id);
        Task<PathwayMetaData> GetPathwayMetadata(string id);
        Task<Pathway> GetIdentifiedPathway(IEnumerable<string> pathwayNumbers, string gender, int age);
        Task<Pathway> GetIdentifiedPathway(string pathwayTitle, string gender, int age);
        Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly);
        Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly, string gender, int age);
        Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly);
        Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly, string gender, int age);
        Task<string> GetSymptomGroup(IList<string> pathwayNos);
        Task<string> GetPathwaysNumbers(string pathwayTitle);
    }

    
}
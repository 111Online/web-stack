using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using NHS111.Models.Models.Domain;

namespace NHS111.Domain.Repository
{
    public class CareAdviceRepository : ICareAdviceRepository
    {
        private readonly IGraphRepository _graphRepository;

        public CareAdviceRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<IEnumerable<CareAdvice>> GetCareAdvice(int age, string gender, IEnumerable<string> markers) // TODO
        {
            string ageGroup = "";
            if (age >= 16) ageGroup = "Adult"; else if (5 <= age && age <= 15) ageGroup = "Child"; else if (1 <= age && age <= 4) ageGroup = "Toddler"; else ageGroup = "Infant";
            // TODO How to deal with infant vs. neonate?

            return await _graphRepository.Client.Cypher.
                Match("(c:CareAdviceText)").
                Where(string.Format("c.id in [{0}]", string.Join(",", markers.Select(marker => string.Format("\"{0}-{1}-{2}\"", marker, ageGroup, gender))))).
                Return(c => c.As<CareAdvice>()).
                ResultsAsync;
            
        }

        public async Task<IEnumerable<CareAdvice>> GetCareAdvice(AgeCategory ageCategory, Gender gender, IEnumerable<string> keywords, DispositionCode dxCode) {
            /*

            MATCH 
            (i:InterimCareAdvice)-[:presentsFor]->(o:Outcome) 
            WHERE 
            i.keyword IN ["Burns and scalds", "Swelling, wounds", "Abdominal pain"] 
            AND 
            o.id = "Dx012" 
            AND 
            i.id =~ ".*-Child-Female" RETURN i, o

            */
            var interimCaNodeName = "InterimCareAdvice";
            var presentsForRelationshipName = "presentsFor";
            var outcomeNodeName = "Outcome";

            return await _graphRepository.Client.Cypher.
                Match(string.Format("(i:{0})-[:{1}]->(o:{2})", interimCaNodeName, presentsForRelationshipName, outcomeNodeName)).
                Where(string.Format("i.keyword in [{0}]", JoinAndEncloseKeywords(keywords))).
                AndWhere(string.Format("o.id = \"{0}\"", dxCode.Value)).
                AndWhere(string.Format("i.id =~ \".*-{0}-{1}\"", ageCategory.Value, gender.Value)).
                AndWhere(BuildExcludeKeywordsWhereStatement(keywords)).
                Return(i => i.As<CareAdvice>()).
                ResultsAsync;
        }


        private string JoinAndEncloseKeywords(IEnumerable<string> keywords) {
            return string.Join(",", keywords.Select(k => k.DoubleQuoted()));
        }

        private string BuildExcludeKeywordsWhereStatement(IEnumerable<string> keywords)
        {
            var whereStatement = "";
            var distinctKeywords = keywords.Distinct();
            foreach (var keyword in distinctKeywords)
            {
                if (distinctKeywords.First() == keyword) whereStatement += "NOT (";
                whereStatement += string.Format("ANY(ex in i.excludeKeywords WHERE ex = {0})", keyword.DoubleQuoted());
                if (distinctKeywords.Last() != keyword) whereStatement += " OR ";
                else whereStatement += ")";
            }
            return whereStatement;
        }

    }


    public static class StringExtensions {
        public static string DoubleQuoted(this string s) {
            return "\"" + s + "\"";
        }
    }

    public interface ICareAdviceRepository {
        Task<IEnumerable<CareAdvice>> GetCareAdvice(int age, string gender, IEnumerable<string> markers);

        Task<IEnumerable<CareAdvice>> GetCareAdvice(AgeCategory ageCategory, Gender gender, IEnumerable<string> keywords,
            DispositionCode dxCode);
    }
}
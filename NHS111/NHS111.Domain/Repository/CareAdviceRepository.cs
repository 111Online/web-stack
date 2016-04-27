using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<CareAdvice>> GetCareAdvice(string ageCategory, string gender, IEnumerable<string> keywords, string dxCode) {
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
                AndWhere(string.Format("o.id = \"{0}\"", dxCode)).
                AndWhere(string.Format("i.id =~ \".*-{0}-{1}\"", ageCategory, gender)).
                Return(i => i.As<CareAdvice>()).
                ResultsAsync;
        }

        private string JoinAndEncloseKeywords(IEnumerable<string> keywords) {
            return string.Join(",", keywords.Select(k => k.DoubleQuoted()));
        }
    }

    public static class StringExtensions {
        public static string DoubleQuoted(this string s) {
            return "\"" + s + "\"";
        }
    }

    public interface ICareAdviceRepository {
        Task<IEnumerable<CareAdvice>> GetCareAdvice(int age, string gender, IEnumerable<string> markers);

        Task<IEnumerable<CareAdvice>> GetCareAdvice(string ageCategory, string gender, IEnumerable<string> keywords,
            string dxCode);
    }
}
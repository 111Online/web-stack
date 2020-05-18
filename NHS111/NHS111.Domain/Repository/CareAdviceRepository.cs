using NHS111.Models.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var adviceWithAllItems = await _graphRepository.Client.Cypher.
                Match("(t:CareAdviceText)-[:hasText*]-(c:InterimCareAdvice)").
                Where(string.Format("c.id in [{0}]", string.Join(",", markers.Select(marker => string.Format("\"{0}-{1}-{2}\"", marker, ageGroup, gender))))).
                Return((c, t) => new CareAdviceFlattened() { CareAdvcieTextDecendants = t.CollectAs<CareAdviceTextWithParent>(), CareAdviceItem = c.As<CareAdvice>() }).ResultsAsync;

            return SoreCareAdviceDescentants(adviceWithAllItems);
        }

        private static List<CareAdvice> SoreCareAdviceDescentants(IEnumerable<CareAdviceFlattened> adviceWithAllItems)
        {
            return adviceWithAllItems.Select(advice => advice.Sort()).ToList();
        }

        public async Task<IEnumerable<CareAdvice>> GetCareAdvice(AgeCategory ageCategory, Gender gender, IEnumerable<string> keywords, DispositionCode dxCode)
        {

            var interimCaNodeName = "InterimCareAdvice";
            var presentsForRelationshipName = "presentsFor";
            var outcomeNodeName = "Outcome";

            var adviceWithAllItemsQuery = _graphRepository.Client.Cypher.
                Match(string.Format("(t:CareAdviceText)-[:hasText*]-(i:{0})-[:{1}]->(o:{2})", interimCaNodeName, presentsForRelationshipName,
                    outcomeNodeName)).
                Where(string.Format("i.keyword in [{0}]", JoinAndEncloseKeywords(keywords))).
                AndWhere(string.Format("o.id = \"{0}\"", dxCode.Value)).
                AndWhere(string.Format("i.id =~ \".*-{0}-{1}\"", ageCategory.Value, gender.Value)).
                AndWhere(BuildExcludeKeywordsWhereStatement(keywords)).
                Return((i, t) => new CareAdviceFlattened() { CareAdvcieTextDecendants = t.CollectAs<CareAdviceTextWithParent>(), CareAdviceItem = i.As<CareAdvice>() });

            return SoreCareAdviceDescentants(await adviceWithAllItemsQuery.ResultsAsync);
        }


        private string JoinAndEncloseKeywords(IEnumerable<string> keywords)
        {
            return string.Join(",", keywords.Select(k => k.DoubleQuoted()));
        }

        private string BuildExcludeKeywordsWhereStatement(IEnumerable<string> keywords)
        {
            var whereStatement = "";
            var distinctKeywords = keywords.Distinct();
            foreach (var keyword in distinctKeywords)
            {
                if (distinctKeywords.First() == keyword) whereStatement += "(i.excludeKeywords IS null OR NOT (";
                whereStatement += string.Format("ANY(ex in i.excludeKeywords WHERE ex = {0})", keyword.DoubleQuoted());
                if (distinctKeywords.Last() != keyword) whereStatement += " OR ";
                else whereStatement += "))";
            }
            return whereStatement;
        }

        public class CareAdviceFlattened
        {
            public CareAdvice CareAdviceItem { get; set; }
            public IEnumerable<CareAdviceTextWithParent> CareAdvcieTextDecendants { get; set; }

            public CareAdvice Sort()
            {
                return new CareAdvice()
                {
                    Id = CareAdviceItem.Id,
                    Title = CareAdviceItem.Title,
                    Keyword = CareAdviceItem.Keyword
                    ,
                    Items = SortTextitemsIntoDescendantHierachy()
                            .OrderBy(i => i.OrderNo).ToList()
                };
            }

            private IEnumerable<CareAdviceText> SortTextitemsIntoDescendantHierachy()
            {
                return CareAdvcieTextDecendants.Where(adviceItems => adviceItems.ParentId == CareAdviceItem.Id)
                    .Select(
                        i =>
                            new CareAdviceText()
                            {
                                Id = i.Id,
                                OrderNo = i.OrderNo,
                                Text = i.Text,
                                Items =
                                    CareAdvcieTextDecendants.Where(adviceItems => adviceItems.ParentId == i.Id)
                                        .Select(childItem => (CareAdviceText)childItem)
                                        .OrderBy(ci => ci.OrderNo)
                                        .ToList()
                            });
            }
        }

    }


    public static class StringExtensions
    {
        public static string DoubleQuoted(this string s)
        {
            return "\"" + s + "\"";
        }
    }

    public interface ICareAdviceRepository
    {
        Task<IEnumerable<CareAdvice>> GetCareAdvice(int age, string gender, IEnumerable<string> markers);

        Task<IEnumerable<CareAdvice>> GetCareAdvice(AgeCategory ageCategory, Gender gender, IEnumerable<string> keywords,
            DispositionCode dxCode);
    }
}


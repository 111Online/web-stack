using System.Linq;
using Neo4jClient.Cypher;
using NHS111.Utils.Configuration;

namespace NHS111.Utils.Extensions
{
    public static class CypherFluentQueryExtension
    {
        public static ICypherFluentQuery Match(this ICypherFluentQuery query, bool onlyLive = false, params string[] matchText)
        {
            if (!onlyLive) return query.Match(matchText);

            return query
                .Match(matchText)
                .Where(string.Format("p.title in [{0}]",
                    string.Join(", ",
                        PathwaysConfigurationManager.GetLivePathwaysElements()
                            .Select(p => string.Format("'{0}'", p.Title)))));
        }

        public static ICypherFluentQuery Where(this ICypherFluentQuery query, string matchText, bool onlyLive = false)
        {
            if (!onlyLive) return query.Where(matchText);

            return query
                .Where(matchText)
                .AndWhere(string.Format("p.title in [{0}]", string.Join(", ", PathwaysConfigurationManager.GetLivePathwaysElements().Select(p => string.Format("'{0}'", p.Title)))));
        }
    }
}

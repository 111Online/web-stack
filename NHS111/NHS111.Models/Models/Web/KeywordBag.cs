using System.Collections.Generic;

namespace NHS111.Models.Models.Web
{
    public class KeywordBag
    {
        public List<Keyword> Keywords { get; set; }
        public List<Keyword> ExcludeKeywords { get; set; }

        public KeywordBag()
        {
            this.Keywords = new List<Keyword>();
            this.ExcludeKeywords = new List<Keyword>();
        }

        public KeywordBag(List<Keyword> keywords, List<Keyword> excludeKeywords)
        {
            this.ExcludeKeywords = excludeKeywords;
            this.Keywords = keywords;
        }
    }

    public class Keyword
    {
        public string Value { get; set; }

        public bool IsFromAnswer { get; set; }
    }
}

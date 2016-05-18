using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class KeywordBag
    {
        public List<string> Keywords { get; set; }
        public List<string> ExcludeKeywords { get; set; }

        public KeywordBag()
        {
            this.Keywords = new List<string>();
            this.ExcludeKeywords = new List<string>();
        }

        public KeywordBag(List<string> keywords, List<string> excludeKeywords)
        {
            this.ExcludeKeywords = excludeKeywords;
            this.Keywords = keywords;
        }
    }
}

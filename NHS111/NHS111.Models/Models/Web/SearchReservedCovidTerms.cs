using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public static class SearchReservedCovidTerms
    {
        private static readonly List<string> Terms = new List<string>()
        {
            "coronavirus",
            "covid",
            "corona",
            "covid 19",
            "corona virus",
            "covid19"
        };

        public static List<string> SearchTerms
        {
            get { return Terms; }
        }
    }
}

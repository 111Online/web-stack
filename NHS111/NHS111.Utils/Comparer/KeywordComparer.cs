using NHS111.Models.Models.Web;
using System.Collections.Generic;

namespace NHS111.Utils.Comparer
{
    public class KeywordComparer : IEqualityComparer<Keyword>
    {

        public bool Equals(Keyword x, Keyword y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(Keyword obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}

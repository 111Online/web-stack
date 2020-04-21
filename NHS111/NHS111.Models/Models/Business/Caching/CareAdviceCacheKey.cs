using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class CareAdviceCacheKey : CacheKeyBase<IEnumerable<CareAdvice>>
    {
        public CareAdviceCacheKey(string ageCategory, string gender, string keywords, string dxCode) 
            :base(string.Format("CareAdvice-{0}-{1}-{2}-{3}", dxCode, ageCategory, gender, keywords.Replace(' ', '_')))
        {}

        public CareAdviceCacheKey(int age, string gender, IEnumerable<string> markers) 
            : base(string.Format("CareAdvice-{0}-{1}-{2}", age, gender, markers))
        {}

        public override bool ValidToAdd(IEnumerable<CareAdvice> value)
        {
            return value != null && value.Any();
        }
    }
}

using System.Collections.Generic;

namespace NHS111.Models.Models.Web.ITK
{
    public class SummaryItem
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public List<Value> Values { get; set; }
    }
}

using System.Collections.Generic;

namespace NHS111.Models.Models.Web.Elements
{
    public class HiddenTextViewModel
    {
        public string Summary { get; set; }

        public IEnumerable<string> Details { get; set; }
    }
}

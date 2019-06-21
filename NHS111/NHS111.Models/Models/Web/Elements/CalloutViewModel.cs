using System.Collections.Generic;

namespace NHS111.Models.Models.Web.Elements
{
    public class CalloutViewModel
    {
        public string Heading { get; set; }

        public IEnumerable<string> Details { get; set; }

        public IEnumerable<string> Modifiers { get; set; }
    }
}

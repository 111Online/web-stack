using System.Collections.Generic;

namespace NHS111.Models.Models.Web.Elements
{
    public class ButtonViewModel
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public IEnumerable<string> Modifiers { get; set; }
    }
}

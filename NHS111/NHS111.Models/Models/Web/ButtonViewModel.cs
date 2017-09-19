using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class ButtonViewModel
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public IEnumerable<string> Modifiers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class HiddenTextViewModel
    {
        public string Summary { get; set; }

        public IEnumerable<string> Details { get; set; }
    }
}

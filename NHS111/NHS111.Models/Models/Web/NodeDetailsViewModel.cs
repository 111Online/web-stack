using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Models.Models.Web
{
    public class NodeDetailsViewModel
    {
        public NodeType NodeType { get; set; }
        public OutcomeGroup OutcomeGroup { get; set; }
    }
}

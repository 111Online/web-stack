using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Business.Question
{
    public class ModZeroJourney
    {
        public string PathwayId { get; set; }
        public string DispositionId { get; set; }
        public IEnumerable<JourneyStep> Steps { get; set; }
    }
}

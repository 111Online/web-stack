using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosFilteredCase : DosCase
    {
        public DateTime DispositionTime { get; set; }

        public int DispositionTimeFrameMinutes { get; set; }
    }
}

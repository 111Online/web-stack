using System;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosFilteredCase : DosCase
    {
        public DateTime DispositionTime { get; set; }

        public int DispositionTimeFrameMinutes { get; set; }
    }
}

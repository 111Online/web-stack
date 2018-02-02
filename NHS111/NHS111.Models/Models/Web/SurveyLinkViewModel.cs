using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class SurveyLinkViewModel
    {
        public string JourneyId { get; set; }

        public string PathwayNo { get; set; }

        public string DigitalTitle { get; set; }

        public string EndPathwayNo { get; set; }

        public string EndPathwayTitle { get; set; }

        public string DispositionCode { get; set; }

        public DateTime DispositionDateTime { get; set; }

        public string Campaign { get; set; }

        public string CampaignSource { get; set; }
    }
}

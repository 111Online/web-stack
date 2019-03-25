using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class RecommendedServiceViewModel : ServiceViewModel
    {
        public string ReasonText { get; set; }
        public DetailsViewModel Details { get; set; }
    }
}

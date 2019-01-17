using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class LocationViewModel : JourneyViewModel
    {
        public string Postcode { get; set; }
    }

    public class ProviderViewModel : LocationViewModel
    {
    }

    public class OutOfAreaViewModel : LocationViewModel
    {
    }
}

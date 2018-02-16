using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class ConfirmLocationViewModel : OutcomeViewModel
    {
        public List<AddressInfoViewModel> FoundLocations { get; set; }

        public string SelectedPostcode { get; set; }
    }
}

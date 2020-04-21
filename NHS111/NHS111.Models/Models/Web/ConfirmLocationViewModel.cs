using System.Collections.Generic;

namespace NHS111.Models.Models.Web
{
    public class ConfirmLocationViewModel : OutcomeViewModel
    {
        public List<AddressInfoViewModel> FoundLocations { get; set; }

        public string SelectedPostcode { get; set; }
    }
}

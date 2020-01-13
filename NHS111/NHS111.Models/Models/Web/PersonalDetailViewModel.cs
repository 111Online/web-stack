using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class PersonalDetailViewModel : OutcomeViewModel
    {
        public LocationInfoViewModel AddressInformation { get; set; }
        public PatientInformantViewModel PatientInformantDetails { get; set; }
        public List<SlotViewModel> Slots { get; set; }
        public string SelectedSlotId { get; set; }
        public bool ConsentGiven { get; set; }
        public SlotViewModel SelectedSlot
        {
            get
            {
                return Slots.Any() ? Slots.FirstOrDefault(s => s.Id == SelectedSlotId) : null;
            }
        }

        public PersonalDetailViewModel() 
        {
            AddressInformation = new LocationInfoViewModel();
            PatientInformantDetails = new PatientInformantViewModel();
            Slots = new List<SlotViewModel>();
        }
    }
}

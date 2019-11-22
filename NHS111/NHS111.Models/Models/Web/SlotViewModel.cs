using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public enum AppointmentSlotStatus
    {
        Busy,
        Free,
        BusyUnavailable,
        BusyTentative,
        EnteredInError
    }
    public class SlotViewModel
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public AppointmentSlotStatus Status { get; set; }
        public string ScheduleId { get; set; }
        public string PractitionerId { get; set; }
    }
}

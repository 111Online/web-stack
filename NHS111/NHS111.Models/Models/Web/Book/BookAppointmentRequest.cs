using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.Book
{
    public class BookAppointmentRequest
    {
        public string SlotId { get; set; }
        public string ScheduleId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Patient Patient { get; set; }
        public string PractitionerId { get; set; }
    }
}

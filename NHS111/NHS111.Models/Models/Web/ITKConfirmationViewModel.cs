using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class ITKConfirmationViewModel :OutcomeViewModel
    {
        public string PatientReference { get; set; }
        public bool? ItkSendSuccess { get; set; }
        public bool? ItkDuplicate { get; set; }
    }
}

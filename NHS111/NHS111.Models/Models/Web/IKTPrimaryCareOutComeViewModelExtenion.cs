using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class ITKPrimaryCareOutComeViewModelExtension
    {

        public static string GetHeaderText(OutcomeViewModel outcomeViewModel)
        {
            switch (outcomeViewModel.Id)
            {
                case "Dx09":
                    return "Book a GP appointment if you don't feel better in a few days";
                case "Dx10":
                    return "Book a non-urgent GP appointment";
                case "Dx16":
                    return "Contact your GP if you don't feel better in a few days";
                case "Dx75":
                    return "Contact your GP within the next few days";
                default:
                    return string.Format("{0} {1}", outcomeViewModel.OutcomeGroup.Text, outcomeViewModel.DispositionUrgencyText);
            }
        }

        public static string GetCannotGetAppointmentText(OutcomeViewModel outcomeViewModel)
        {
            switch (outcomeViewModel.Id)
            {
                case "Dx08":
                case "Dx09":
                case "Dx10":
                case "Dx15":
                case "Dx16":
                case "Dx75":
                    return "I can't get an appointment today or tomorrow";
                default:
                    return "I can't get an appointment today";
            }
        }
    }
}


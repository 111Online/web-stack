using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Models.Models.Web
{
    public class ITKPrimaryCareOutComeViewModelExtension
    {

        public static string GetHeaderText(OutcomeViewModel outcomeViewModel)
        {
            var dispositionsWithCustomHeading = new Dictionary<string, string>()
            {
                { "Dx09" ,"Book a GP appointment if you don't feel better in a few days"},
                { "Dx10" ,"Book a non-urgent GP appointment"},
                { "Dx16" ,"Contact your GP if you don't feel better in a few days"},
                { "Dx75" ,"Contact your GP within the next few days"},
            };
           
            return dispositionsWithCustomHeading.ContainsKey(outcomeViewModel.Id) 
                ?
                    dispositionsWithCustomHeading[outcomeViewModel.Id] 
                : 
                    string.Format("{0} {1}", outcomeViewModel.OutcomeGroup.Text, outcomeViewModel.DispositionUrgencyText);
        }

        public static bool ShouldDisplayPharmacistReferral(OutcomeViewModel outcomeViewModel)
        {
            var hasEmergencyNationalResponseDosServiceType =
                outcomeViewModel.DosCheckCapacitySummaryResult.ContainsServiceTypeById(138);

            var dispositionIdsWithPharmacistReferral = new List<string>()
            {
                "Dx09", "Dx10", "Dx16", "Dx75"
            };

            return hasEmergencyNationalResponseDosServiceType && dispositionIdsWithPharmacistReferral.Contains(outcomeViewModel.Id);
        }

        public static ServiceViewModel SelectedServiceForPharmacistReferral(OutcomeViewModel outcomeViewModel)
        {
            return outcomeViewModel.DosCheckCapacitySummaryResult.GetFirstServiceByServiceTypeId(138);
        }


        public static bool ShouldDisplayForNonUrgentDispositions(string dispositionId)
        {
            var NonUrgentDispositions = new List<string>()
            {
                "Dx09", "Dx10", "Dx16", "Dx75"
            };
            return NonUrgentDispositions.Contains(dispositionId);
        }



        public static string GetCannotGetAppointmentText(string dispositionId)
        {
            var dispositionWithCustomCannotGetAppointmentText = new Dictionary<string, string>()
            {
                { "Dx08" ,"I can't get an appointment today or tomorrow"},
                { "Dx15" ,"I can't get an appointment today or tomorrow"},
            };
            return dispositionWithCustomCannotGetAppointmentText.ContainsKey(dispositionId)
                ? 
                    dispositionWithCustomCannotGetAppointmentText[dispositionId]
                : 
                    "I can't get an appointment today";
        }
    }
}


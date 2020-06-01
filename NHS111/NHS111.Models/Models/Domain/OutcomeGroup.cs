using System;
using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    using Newtonsoft.Json;
    using System.Linq;

    public class OutcomeGroup : IEquatable<OutcomeGroup>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        public string Label { get; set; }

        public string DefaultTitle { get; set; }

        public string SearchDestination
        {
            get { return ITK ? "ServiceList" : "ServiceDetails"; }
        }

        public bool ITK { get; set; }

        public bool CanPatientsRegisterWithService { get; set; }

        public bool RequiresEmail { get; set; }

        public static OutcomeGroup ClinicianCallBack = new OutcomeGroup { Id = "ITK_Clinician_call_back", Text = "ITK_Clinician_call_back", AutomaticSelectionOfItkResult = true, DefaultTitle = "Based on your answers, we recommend that you speak to a clinician", Label = "Healthcare services", ITK = true };

        public static OutcomeGroup ItkPrimaryCare = new OutcomeGroup { Id = "ITK_Primary_care", Text = "ITK_Primary_care", PostcodeFirst = true, CanPatientsRegisterWithService = true, DefaultTitle = "Based on your answers, we recommend you speak to a healthcare service" };
        
        public static OutcomeGroup ItkPrimaryCareNer = new OutcomeGroup { Id = "ITK_Primary_care_NER", Text = "ITK_Primary_care_NER", PostcodeFirst = true, CanPatientsRegisterWithService = true, DefaultTitle = "Based on your answers, we recommend you speak to a healthcare service" };

        public static OutcomeGroup Call999Police = new OutcomeGroup { Id = "Call_999_police", Text = "Call_999_police", DefaultTitle = "Your answers suggest you should dial 999 now for the police" };

        public static OutcomeGroup Call999Cat1 = new OutcomeGroup { Id = "Call_999_cat_1", Text = "Phone 999 now for an ambulance" };

        public static OutcomeGroup Call999Cat2 = new OutcomeGroup { Id = "Call_999_cat_2", Text = "Phone 999 now for an ambulance" };

        public static OutcomeGroup Call999Cat3 = new OutcomeGroup { Id = "Call_999_cat_3", Text = "Call_999_cat_3", DefaultTitle = "Phone 999 now for an ambulance", AutomaticSelectionOfItkResult = true };

        public static OutcomeGroup Call999Cat4 = new OutcomeGroup { Id = "Call_999_cat_4", Text = "Call_999_cat_4", DefaultTitle = "Phone 999 for an ambulance", AutomaticSelectionOfItkResult = true };

        public static OutcomeGroup MentalHealth = new OutcomeGroup { Id = "SP_Mental_health", DefaultTitle = "Get help from a mental health service", Label = "Mental health services", ITK = false };

        public static OutcomeGroup AccidentAndEmergency = new OutcomeGroup { Id = "SP_Accident_and_emergency", DefaultTitle = "Your answers suggest you should go to an Accident and Emergency department", Label = "Urgent healthcare services", ITK = false };

        public static OutcomeGroup AccidentAndEmergencySexualAssault = new OutcomeGroup { Id = "SP_Accident_and_emergency_sexual_assault", DefaultTitle = "Your answers suggest you should go to an Accident and Emergency department", Label = "A&amp;E departments", ITK = false };

        public static OutcomeGroup HomeCare = new OutcomeGroup { Id = "Home_Care", Text = "Home Care" };

        public static OutcomeGroup Pharmacy = new OutcomeGroup { Id = "SP_Pharmacy", Text = "Pharmacy", DefaultTitle = "Your answers suggest you should see a pharmacist", Label = "Pharmacies", ITK = false };

        public static OutcomeGroup GumClinic = new OutcomeGroup { Id = "SP_GUM_Clinic", Text = "Sexual Health Clinic", DefaultTitle = "Your answers suggest you should visit a sexual health clinic", Label = "Sexual health clinics", ITK = false };

        public static OutcomeGroup Optician = new OutcomeGroup { Id = "SP_Optician", Text = "Optician", DefaultTitle = "Your answers suggest you should see an optician", Label = "Opticians", ITK = false };

        public static OutcomeGroup Dental = new OutcomeGroup { Id = "SP_Dental", Text = "Dental treatment centre", PostcodeFirst = true, CanPatientsRegisterWithService = true, DefaultTitle = "Your answers suggest you should get dental treatment" };

        public static OutcomeGroup EmergencyDental = new OutcomeGroup { Id = "SP_Emergency_dental", Text = "Emergency dental treatment centre", DefaultTitle = "Your answers suggest you should get emergency dental treatment", Label = "Emergency dental treatment centres", ITK = false };

        public static OutcomeGroup Midwife = new OutcomeGroup { Id = "SP_Midwife", Text = "SP_Midwife", DefaultTitle = "Your answers suggest you should speak to your midwife", Label = "Antenatal services", ITK = true };

        public static OutcomeGroup GP = new OutcomeGroup { Id = "SP_GP", Text = "SP_GP", DefaultTitle = "Based on your answers, we recommend you speak to a healthcare service", Label = "Healthcare services" };

        public static OutcomeGroup RepeatPrescription = new OutcomeGroup { Id = "Repeat_Prescription", Text = "Repeat_Prescription", DefaultTitle = "Where to go for help", Label = "Repeat Prescription", PostcodeFirst = true, ITK = true };

        public static OutcomeGroup NoFurtherAction = new OutcomeGroup { Id = "No_Further_Action", Text = "No_Further_Action", DefaultTitle = "No further action required", Label = "No further action", PostcodeFirst = true, ITK = false };

        public static OutcomeGroup JumpToSearch = new OutcomeGroup { Id = "111_Search_Jump", Text = "", DefaultTitle = "", Label = "" };

        #region Corona virus specific outcomes
        public static OutcomeGroup Isolate111 = new OutcomeGroup { Id = "Isolate_111", Text = "Isolate_111", DefaultTitle = "Isolate yourself at home", Label = "No further action", PostcodeFirst = false, ITK = false, RequiresEmail = true };
        public static OutcomeGroup Isolate_SelfCare = new OutcomeGroup { Id = "Isolate_SelfCare", Text = "Isolate_SelfCare", DefaultTitle = "Isolate yourself at home", Label = "No further action", PostcodeFirst = false, ITK = false };
        public static OutcomeGroup Isolate_999 = new OutcomeGroup { Id = "Isolate_999", Text = "Isolate_999", DefaultTitle = "Isolate yourself at home", Label = "Call 999", PostcodeFirst = false, ITK = false };
        #endregion

        #region Send SMS outcomes
        public static OutcomeGroup Send_SMS = new OutcomeGroup { Id = "Send_SMS", Text = "Send_SMS", DefaultTitle = "Send sms service", Label = "No further action", PostcodeFirst = false, ITK = false, RequiresEmail = false };
        public static OutcomeGroup Verify_SMS = new OutcomeGroup { Id = "Verify_SMS", Text = "Verify_SMS", DefaultTitle = "Verify sms service", Label = "No further action", PostcodeFirst = false, ITK = false, RequiresEmail = false };
        #endregion

        public static OutcomeGroup[] PrePopulatedDosResultsOutcomeGroups = new OutcomeGroup[] {Dental, ItkPrimaryCare, ItkPrimaryCareNer, GP, MentalHealth, AccidentAndEmergency, ClinicianCallBack, Call999Cat3, Call999Cat4, RepeatPrescription, Isolate111 };
        public static OutcomeGroup[] DosSearchOutcomesGroups = new OutcomeGroup[] { MentalHealth, AccidentAndEmergency, AccidentAndEmergencySexualAssault, Optician, Pharmacy, GumClinic, Dental, EmergencyDental, Midwife, ItkPrimaryCare, ItkPrimaryCareNer, ClinicianCallBack };
        public static OutcomeGroup[] UsingRecommendedServiceJourney = new[] { RepeatPrescription };
        public static OutcomeGroup[] UsingOutcomePreamble = new[] { RepeatPrescription };

        public static readonly Dictionary<string, OutcomeGroup> OutcomeGroups = new Dictionary<string, OutcomeGroup>()
        {
            { ClinicianCallBack.Id, ClinicianCallBack},
            { ItkPrimaryCare.Id, ItkPrimaryCare},
            { ItkPrimaryCareNer.Id, ItkPrimaryCareNer},
            { Call999Cat1.Id, Call999Cat1 },
            { Call999Cat2.Id, Call999Cat2 },
            { Call999Cat3.Id, Call999Cat3 },
            { Call999Cat4.Id, Call999Cat4 },
            { Call999Police.Id, Call999Police },
            { MentalHealth.Id, MentalHealth },
            { AccidentAndEmergency.Id, AccidentAndEmergency },
            { AccidentAndEmergencySexualAssault.Id, AccidentAndEmergencySexualAssault },
            { HomeCare.Id, HomeCare },
            { Pharmacy.Id, Pharmacy },
            { GumClinic.Id, GumClinic },
            { Optician.Id, Optician },
            { Dental.Id, Dental },
            { EmergencyDental.Id, EmergencyDental },
            { Midwife.Id, Midwife },
            { GP.Id, GP },
            { RepeatPrescription.Id, RepeatPrescription },
            { NoFurtherAction.Id, NoFurtherAction },
            { JumpToSearch.Id, JumpToSearch },

            #region Corona virus specific outcomes
            { Isolate111.Id, Isolate111 },
            { Isolate_SelfCare.Id, Isolate_SelfCare },
            { Isolate_999.Id, Isolate_999 },
            #endregion

            #region Sebd SMS outcomes
            { Send_SMS.Id, Send_SMS },
            { Verify_SMS.Id, Verify_SMS }
            #endregion
        };

        public override bool Equals(object obj)
        {
            var outcomeGroup = obj as OutcomeGroup;
            if (outcomeGroup == null)
                return false;

            return this.Equals(outcomeGroup);
        }

        public bool Equals(OutcomeGroup group)
        {
            if (group == null)
                return false;

            return Id == group.Id;
        }

        public override int GetHashCode()
        {
            return Id == null ? 0 : Id.GetHashCode();
        }

        public string GetDefaultTitle()
        {
            return OutcomeGroups.ContainsKey(Id) ? OutcomeGroups[Id].DefaultTitle : "Search results";
        }

        public bool IsAutomaticSelectionOfItkResult()
        {
            if (Id == null) return false;
            return OutcomeGroups.ContainsKey(Id) && OutcomeGroups[Id].AutomaticSelectionOfItkResult;
        }

        public bool CanPatientsRegister()
        {
            if (Id == null) return false;
            return OutcomeGroups.ContainsKey(Id) && OutcomeGroups[Id].CanPatientsRegisterWithService;
        }

        private bool PostcodeFirst { get; set; }
        private bool AutomaticSelectionOfItkResult { get; set; }

        public bool Is999NonUrgent
        {
            get
            {
                return this.Equals(OutcomeGroup.Call999Cat3) ||
                       this.Equals(OutcomeGroup.Call999Cat4);
            }
        }

        public bool IsEDCallback
        {
            get { return this.Equals(OutcomeGroup.AccidentAndEmergency) || this.Equals(OutcomeGroup.MentalHealth); }
        }

        public bool IsPharmacyGroup
        {
            get { return this.Equals(OutcomeGroup.RepeatPrescription); }
        }

        public bool IsPrimaryCare
        {
            get { return this.Equals(OutcomeGroup.ItkPrimaryCare) || this.Equals(OutcomeGroup.ItkPrimaryCareNer) || this.Equals(OutcomeGroup.GP); }
        }

        public bool IsUsingRecommendedService
        {
            get { return UsingRecommendedServiceJourney.Contains(this); }
        }

        public bool IsCoronaVirus
        {
            get
            {
                return Equals(Isolate111) || Equals(Isolate_SelfCare) || Equals(Isolate_999);
            }
        }

        public bool CoronaVirusServiceDetailsMessageDontGo
        {
            get
            {
                return this.Equals(GumClinic);
            }
        }

        public bool CoronaVirusServiceDetailsMessageDontGoOptician
        {
            get
            {
                return this.Equals(Optician);
            }
        }

        public bool CoronaVirusServiceDetailsMessageLongWait
        {
            get
            {
                return this.Equals(EmergencyDental) || this.Equals(AccidentAndEmergencySexualAssault);
            }
        }

        public bool CoronaVirusServiceListMessageBlank
        {
            get
            {
                return this.Equals(Midwife);
            }
        }

        public bool IsSendSMS
        {
            get
            {
                return Equals(Send_SMS);
            }
        }

        public bool IsSendSMSVerify
        {
            get
            {
                return Equals(Verify_SMS);
            }
        }

        public bool IsVerifySMS
        {
            get
            {
                return Equals(Verify_SMS);
            }
        }

        public bool IsCoronaVirusCallback
        {
            get { return Equals(Isolate111); }
        }

        public bool RequiresOutcomePreamble(bool hasViewed)
        {
            return UsingOutcomePreamble.Contains(this) && !hasViewed;
        }
    }
}

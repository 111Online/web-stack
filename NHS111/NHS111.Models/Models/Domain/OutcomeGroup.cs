﻿
using System;
using System.Collections.Generic;

namespace NHS111.Models.Models.Domain {
    using Newtonsoft.Json;

    public class OutcomeGroup : IEquatable<OutcomeGroup>
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        public string Label { get; set; }

        public string DefaultTitle { get; set; }
        public static OutcomeGroup Call999 = new OutcomeGroup { Id = "Call_999", Text = "Call_999", DefaultTitle = "Your answers suggest you need to dial 999 immediately and ask for an ambulance" };

        public static OutcomeGroup AccidentAndEmergency = new OutcomeGroup { Id = "SP_Accident_and_emergency", DefaultTitle = "Your answers suggest you should go to an Accident and Emergency department" };

        public static OutcomeGroup AccidentAndEmergencySexualAssault = new OutcomeGroup { Id = "SP_Accident_and_emergency_sexual_assault", DefaultTitle = "Your answers suggest you should go to an Accident and Emergency department" };

        public static OutcomeGroup HomeCare = new OutcomeGroup { Id = "Home_Care", Text = "Home Care"};

        public static OutcomeGroup Pharmacy = new OutcomeGroup { Id = "SP_Pharmacy", Text = "Pharmacy", DefaultTitle = "Your answers suggest you should see a pharmacist" };

        public static OutcomeGroup GumClinic = new OutcomeGroup { Id = "SP_GUM_Clinic", Text = "Sexual Health Clinic", DefaultTitle = "Your answers suggest you should visit a sexual health clinic" };

        public static OutcomeGroup Optician = new OutcomeGroup { Id = "SP_Optician", Text = "Optician", DefaultTitle = "Your answers suggest you should see an optician" };

        public static OutcomeGroup Dental = new OutcomeGroup { Id = "SP_Dental", Text = "Dental treatment centre", DefaultTitle = "Your answers suggest you should get dental treatment" };

        public static OutcomeGroup EmergencyDental = new OutcomeGroup { Id = "SP_Emergency_dental", Text = "Emergency dental treatment centre", DefaultTitle = "Your answers suggest you should get emergency dental treatment" };

        public static OutcomeGroup[] SignpostingOutcomesGroups = new OutcomeGroup[] { AccidentAndEmergency, AccidentAndEmergencySexualAssault, Optician, Pharmacy, GumClinic, Dental, EmergencyDental };
     
        private static readonly Dictionary<string, OutcomeGroup> OutcomeGroups = new Dictionary<string, OutcomeGroup>()
        {
            { Call999.Id, Call999 },
            { AccidentAndEmergency.Id, AccidentAndEmergency },
            { AccidentAndEmergencySexualAssault.Id, AccidentAndEmergencySexualAssault },
            { HomeCare.Id, HomeCare },
            { Pharmacy.Id, Pharmacy },
            { GumClinic.Id, GumClinic },
            { Optician.Id, Optician },
            { Dental.Id, Dental },
            { EmergencyDental.Id, EmergencyDental }
        };

        public override bool Equals(object obj) {
            var outcomeGroup = obj as OutcomeGroup;
            if (outcomeGroup == null)
                return false;

            return this.Equals(outcomeGroup);
        }

        public bool Equals(OutcomeGroup group) {
            if (group == null)
                return false;

            return Id == group.Id;
        }

        public override int GetHashCode() {
            return Id == null ? 0 : Id.GetHashCode();
        }

        public string GetDefaultTitle()
        {
            return OutcomeGroups.ContainsKey(Id) ? OutcomeGroups[Id].DefaultTitle : "Search results";
        }
    }
}
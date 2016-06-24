﻿
namespace NHS111.Models.Models.Domain {
    using Newtonsoft.Json;

    public class OutcomeGroup {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        public static OutcomeGroup Call999 = new OutcomeGroup { Text = "Call_999" };
        public static OutcomeGroup AccidentAndEmergency = new OutcomeGroup { Text = "SP_Accident_and_emergency" };
        public static OutcomeGroup HomeCare = new OutcomeGroup { Text = "Home_Care" };

        public override bool Equals(object obj) {
            var outcomeGroup = obj as OutcomeGroup;
            if (outcomeGroup == null)
                return false;

            return Text == outcomeGroup.Text;
        }

        public bool Equals(OutcomeGroup group) {
            if (group == null)
                return false;

            return Text == group.Text;
        }

        public override int GetHashCode() {
            return Id == null ? 0 : Id.GetHashCode();
        }
    }
}
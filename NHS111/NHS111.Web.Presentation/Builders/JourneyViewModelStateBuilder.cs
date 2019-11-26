using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;

namespace NHS111.Web.Presentation.Builders
{
    public static class JourneyViewModelStateBuilder
    {
        public static IDictionary<string, string> BuildState(string journeyId, string gender, int age)
        {
            return BuildState(journeyId, gender, age, new Dictionary<string, string>());
        }

        public static IDictionary<string, string> BuildState(string journeyId, string gender, int age, IDictionary<string, string> state)
        {
            AgeCategory ageCategory = new AgeCategory(age);

            Normalise(state);

            state.Add("PATIENT_AGE", age.ToString());
            state.Add("PATIENT_GENDER", string.Format("\"{0}\"", gender.First().ToString().ToUpper()));
            state.Add("PATIENT_PARTY", "1");
            state.Add("PATIENT_AGEGROUP", ageCategory.Value);
            if(!Guid.Empty.ToString().Equals(journeyId))
                state.Add("JOURNEY_ID", journeyId);

            return state;
        }

        public static string BuildStateJson(IDictionary<string, string> state)
        {
            return JsonConvert.SerializeObject(state);
        }

        private static void Normalise(IDictionary<string, string> state)
        {
            if (state.ContainsKey("PATIENT_AGE"))
                state.Remove("PATIENT_AGE");
            if (state.ContainsKey("PATIENT_GENDER"))
                state.Remove("PATIENT_GENDER");
            if (state.ContainsKey("PATIENT_PARTY"))
                state.Remove("PATIENT_PARTY");
            if (state.ContainsKey("PATIENT_AGEGROUP"))
                state.Remove("PATIENT_AGEGROUP");
            if (state.ContainsKey("JOURNEY_ID"))
                state.Remove("JOURNEY_ID");
        }

    }
}

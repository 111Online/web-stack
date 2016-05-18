using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Web.Presentation.Builders
{
    public static class JourneyViewModelStateBuilder
    {
        public static IDictionary<string, string> BuildState(string gender, int age, IMappingEngine mappingEngine)
        {
            return BuildState(gender, age, mappingEngine, new Dictionary<string, string>());
        }

        public static IDictionary<string, string> BuildState(string gender, int age, IMappingEngine mappingEngine, IDictionary<string, string> state)
        {
            AgeCategory ageCategory = mappingEngine.Map<AgeCategory>(age);

            state.Add("PATIENT_AGE", age.ToString());
            state.Add("PATIENT_GENDER", string.Format("\"{0}\"", gender.First().ToString().ToUpper()));
            state.Add("PATIENT_PARTY", "1");
            state.Add("PATIENT_AGEGROUP", ageCategory.ToString());

            return state;
        }

        public static string BuildStateJson(IDictionary<string, string> state)
        {
            return JsonConvert.SerializeObject(state);
        }
    }
}

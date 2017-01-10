using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Utils.Parser
{
    public static class JourneyJsonParser
    {
        public static string GetEndingPathway(string journeyJson)
        {
            var journey = JsonConvert.DeserializeObject<Journey>(journeyJson);
            var endingPathway = journey.Steps.Last();

            return endingPathway.QuestionId.Split('.').FirstOrDefault();
        }
    }
}

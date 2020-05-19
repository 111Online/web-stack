using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Web.Clock;
using System;
using System.Collections.Generic;

namespace NHS111.Utils.Converters
{
    public class DosServiceConverter : JsonConverter
    {
        private readonly IClock _clock;

        public DosServiceConverter(IClock clock)
        {
            _clock = clock;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load the JSON for the Result into a JArray
            var servicesArray = JArray.Load(reader);
            var dosServices = new List<Models.Models.Business.DosService>();
            foreach (var service in servicesArray)
            {
                var dosService = new Models.Models.Business.DosService(_clock);
                serializer.Populate(service.CreateReader(), dosService);
                dosServices.Add(dosService);
            }

            // Return the result
            return dosServices;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IList<Models.Models.Business.DosService>));
        }
    }
}

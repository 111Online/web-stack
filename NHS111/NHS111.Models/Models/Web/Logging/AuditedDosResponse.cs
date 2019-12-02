using System.Collections.Generic;
using NHS111.Models.Models.Web.FromExternalServices;
using System.Linq;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditedDosResponse
    {
        public bool DosResultsContainItkOfferring { get; set; }
        public ResponseObject Success { get; set; }
        public ResponseObject Error { get; set; }
    }

    public class ResponseObject
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
    }
}

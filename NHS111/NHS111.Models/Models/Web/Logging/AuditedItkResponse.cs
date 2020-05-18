using System.Net;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditedItkResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}

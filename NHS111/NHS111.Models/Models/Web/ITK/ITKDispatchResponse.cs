
namespace NHS111.Models.Models.Web.ITK
{
    using System.Net.Http;

    public class ITKDispatchResponse
        : HttpResponseMessage
    {
        public string Body { get; set; } //this can be changed to a more complex type as required.
    }

}

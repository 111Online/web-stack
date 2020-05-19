
namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using System.Net;

    public static class EsbStatusCode
    {
        public static HttpStatusCode Error500
        {
            get { return HttpStatusCode.InternalServerError; }
        }

        public static HttpStatusCode Success200
        {
            get { return HttpStatusCode.OK; }
        }

        public static HttpStatusCode Duplicate409
        {
            get { return HttpStatusCode.Conflict; }
        }
    }
}
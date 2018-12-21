namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System.Net;

    public abstract class EsbVerificationResult {
        public abstract HttpStatusCode StatusCode { get; }
    }

    public class SuccessfulEsbVerificationResult
        : EsbVerificationResult {

        public override HttpStatusCode StatusCode {
            get { return HttpStatusCode.OK; }
        }
    }


}
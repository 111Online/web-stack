namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using Models.Models.Business;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public abstract class VerificationResult
    {
        public abstract HttpStatusCode StatusCode { get; }
        public abstract string FailureReason { get; }
    }

    public class SuccessfulVerificationResult
        : VerificationResult
    {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.OK; } }
        public override string FailureReason { get { return null; } }
    }

    public class NoMatchingScenarioVerificationResult
        : VerificationResult
    {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.NotFound; } }
        public Postcode Postcode { get; private set; }

        public override string FailureReason
        {
            get
            {
                if (Postcode == null)
                    return "Cannot verify a scenario with no postcode.";

                return string.Format("No scenario found matching {0}. Was the scenario successfully begun?",
                    Postcode.NormalisedValue);
            }
        }

        public NoMatchingScenarioVerificationResult(Postcode postcode)
        {
            Postcode = postcode;
        }
    }

    public class IncorrectNumberOfRequestsVerificationResult<T>
        : VerificationResult
    {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.Conflict; } }
        public Postcode Postcode { get; private set; }
        public IEnumerable<T> ExpectedRequests { get; private set; }
        public IEnumerable<RequestAuditRecord<T>> RequestAudit { get; private set; }

        public override string FailureReason
        {
            get
            {
                return
                    string.Format("The scenario for {0} expected {1} requests but it received {2} requests.",
                        Postcode.NormalisedValue, ExpectedRequests.Count(), RequestAudit.Count());
            }
        }

        public IncorrectNumberOfRequestsVerificationResult(Postcode postcode, IEnumerable<T> expectedRequests, IEnumerable<RequestAuditRecord<T>> requestAudit)
        {
            ExpectedRequests = expectedRequests;
            Postcode = postcode;
            RequestAudit = requestAudit;
        }
    }

    public class RequestMismatchVerificationResult<T>
        : VerificationResult
    {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.BadRequest; } }
        public Postcode Postcode { get; private set; }
        public IEnumerable<RequestAuditRecord<T>> RequestAudit { get; private set; }

        public IEnumerable<RequestAuditRecord<T>> GetMismatchingRequests()
        {
            return RequestAudit.Where(r => !r.Matched);
        }

        public RequestAuditRecord<T> GetFirstMismatchingRequest()
        {
            return RequestAudit.First(r => !r.Matched);
        }

        public override string FailureReason
        {
            get
            {
                return string.Format("One or more unexpected requests were made to scenario {0}.", Postcode);
            }
        }

        public RequestMismatchVerificationResult(Postcode postcode, IEnumerable<RequestAuditRecord<T>> requestAudit)
        {
            Postcode = postcode;
            RequestAudit = requestAudit;
        }
    }

}
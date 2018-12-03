namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Models.Models.Business;

    public abstract class DosVerificationResult {
        public abstract HttpStatusCode StatusCode { get;  }
        public abstract string FailureReason { get; }
    }

    public class SuccessfulDosVerificationResult
        : DosVerificationResult {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.OK; } }
        public override string FailureReason { get { return null; } }
    }

    public class NoMatchingScenarioDosVerificationResult
        : DosVerificationResult {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.NotFound; } }
        public Postcode Postcode { get; private set; }

        public override string FailureReason {
            get {
                return string.Format("No scenario found matching {0}. Was the scenario successfully begun?",
                    Postcode.NormalisedValue);
            }
        }

        public NoMatchingScenarioDosVerificationResult(Postcode postcode) {
            Postcode = postcode;
        }
    }

    public class IncorrectNumberOfRequestsDosVerificationResult
        : DosVerificationResult {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.Conflict; } }
        public DosTestScenario Scenario { get; private set; }
        public IEnumerable<DosRequestAuditRecord> RequestAudit { get; private set; }

        public override string FailureReason {
            get {
                return
                    string.Format("The scenario for {0} expected {1} requests but it received {2} requests.",
                        Scenario.Postcode, Scenario.Requests.Count, RequestAudit.Count());
            }
        }

        public IncorrectNumberOfRequestsDosVerificationResult(DosTestScenario scenario, IEnumerable<DosRequestAuditRecord> requestAudit) {
            Scenario = scenario;
            RequestAudit = requestAudit;
        }
    }

    public class RequestMismatchDosVerificationResult
        : DosVerificationResult {
        public override HttpStatusCode StatusCode { get { return HttpStatusCode.BadRequest; } }
        public DosTestScenario Scenario { get; private set; }
        public IEnumerable<DosRequestAuditRecord> RequestAudit { get; private set; }

        public IEnumerable<DosRequestAuditRecord> GetMismatchingRequests() {
            return RequestAudit.Where(r => !r.Matched);
        }

        public DosRequestAuditRecord FirstMismatchingRequest() {
            return RequestAudit.First(r => !r.Matched);
        }

        public override string FailureReason {
            get {
                return string.Format("One or more unexpected requests were made to scenario {0}.", Scenario.Postcode);
            }
        }

        public RequestMismatchDosVerificationResult(DosTestScenario scenario,
            IEnumerable<DosRequestAuditRecord> requestAudit) {
            Scenario = scenario;
            RequestAudit = requestAudit;
        }
    }

}
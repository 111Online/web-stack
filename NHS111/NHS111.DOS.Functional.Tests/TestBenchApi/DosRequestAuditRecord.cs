namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using Models.Models.Web.DosRequests;

    public class DosRequestAuditRecord {
        public bool Matched { get; set; }
        public DosFilteredCase Request { get; set; }

        public DosRequestAuditRecord() { }

        protected DosRequestAuditRecord(bool matched, DosFilteredCase request) {
            Matched = matched;
            Request = request;
        }
    }

    public class MatchingDosRequestAuditRecord
        : DosRequestAuditRecord {

        public MatchingDosRequestAuditRecord(DosFilteredCase request)
            : base(true, request) { }
    }

    public class MismatchingDosRequestAuditRecord
        : DosRequestAuditRecord {

        public MismatchingDosRequestAuditRecord(DosFilteredCase request)
            : base(false, request) { }
    }
}
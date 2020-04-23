namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    public class RequestAuditRecord<T>
    {
        public bool Matched { get; set; }
        public T Request { get; set; }

        public RequestAuditRecord() { }

        protected RequestAuditRecord(bool matched, T request)
        {
            Matched = matched;
            Request = request;
        }
    }

    public class MatchingRequestAuditRecord<T>
        : RequestAuditRecord<T>
    {

        public MatchingRequestAuditRecord(T request)
            : base(true, request) { }
    }

    public class MismatchingRequestAuditRecord<T>
        : RequestAuditRecord<T>
    {

        public MismatchingRequestAuditRecord(T request)
            : base(false, request) { }
    }
}
namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System.Collections.Generic;
    using Models.Models.Web.ITK;

    public interface IEsbTestScenario
        : ITestScenario<ITKDispatchRequest> {
        ITKDispatchRequest IncomingITKDispatchRequest { get; set; }
        int MatchStatusCode { get; set; }
        int MismatchStatusCode { get; set; }
    }

    public class EsbTestScenario
        : IEsbTestScenario {
        public ITKDispatchRequest IncomingITKDispatchRequest { get; set; }
        public int MatchStatusCode { get; set; }
        public int MismatchStatusCode { get; set; }

        public string Postcode {
            get {
                if (IncomingITKDispatchRequest == null || IncomingITKDispatchRequest.PatientDetails == null || IncomingITKDispatchRequest.PatientDetails.CurrentAddress == null)
                    return null;
                return IncomingITKDispatchRequest.PatientDetails.CurrentAddress.PostalCode;
            }
        }

        public ICollection<ITKDispatchRequest> Requests { get { return new[] {IncomingITKDispatchRequest}; } }
    }
}
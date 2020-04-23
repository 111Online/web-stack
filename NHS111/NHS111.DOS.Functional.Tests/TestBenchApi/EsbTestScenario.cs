namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using Models.Models.Web.ITK;
    using System.Collections.Generic;

    public interface IEsbTestScenario
        : ITestScenario<EsbTestScenarioRequest>
    {
        ITKDispatchRequest IncomingITKDispatchRequest { get; set; }
        int MatchStatusCode { get; set; }
        int MismatchStatusCode { get; set; }
    }

    public class EsbTestScenario
        : IEsbTestScenario
    {
        public ITKDispatchRequest IncomingITKDispatchRequest { get; set; }
        public int MatchStatusCode { get; set; }
        public int MismatchStatusCode { get; set; }

        public string Postcode
        {
            get
            {
                if (IncomingITKDispatchRequest == null || IncomingITKDispatchRequest.PatientDetails == null || IncomingITKDispatchRequest.PatientDetails.CurrentAddress == null)
                    return null;
                return IncomingITKDispatchRequest.PatientDetails.CurrentAddress.PostalCode;
            }
        }

        public ICollection<EsbTestScenarioRequest> Requests { get; set; }
    }

    public class EsbTestScenarioRequest
    {
        public ITKDispatchRequest InboundITKDispatchRequest { get; set; }

        public bool Expected { get; set; }
    }
}
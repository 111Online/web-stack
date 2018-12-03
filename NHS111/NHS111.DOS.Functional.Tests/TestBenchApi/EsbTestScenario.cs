namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using Models.Models.Web.ITK;

    public interface IEsbTestScenario {
        ITKDispatchRequest IncomingITKDispatchRequest { get; set; }
        int MatchStatusCode { get; set; }
        int MismatchStatusCode { get; set; }
    }

    public class EsbTestScenario
        : IEsbTestScenario {
        public ITKDispatchRequest IncomingITKDispatchRequest { get; set; }
        public int MatchStatusCode { get; set; }
        public int MismatchStatusCode { get; set; }
    }
}
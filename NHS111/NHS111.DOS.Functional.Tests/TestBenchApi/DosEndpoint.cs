namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    public interface IDosEndpoint { }

    public static class DosEndpoint {
        public static IDosEndpoint CheckCapacitySummary { get; set; }
    }
}
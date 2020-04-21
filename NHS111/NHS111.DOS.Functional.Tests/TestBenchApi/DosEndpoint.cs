namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    public interface IDosEndpoint { }

    public class DosEndpoint
        : IDosEndpoint
    {
        public static IDosEndpoint CheckCapacitySummary { get { return new DosEndpoint(); } }
    }
}
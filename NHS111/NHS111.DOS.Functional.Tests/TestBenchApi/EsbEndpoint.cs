namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    public interface IEsbEndpoint { }

    public class EsbEndpoint
        : IEsbEndpoint
    {
        public static IEsbEndpoint SendItkMessage { get { return new EsbEndpoint(); } }
    }
}
namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    public interface IEsbEndpoint { }

    public class EsbEndpoint {
        public static IEsbEndpoint SendItkMessage { get; set; }
    }
}
namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    public interface IDosTestScenarioTransformer
    {
        string Name { get; }
        object[] Arguments { get; }
    }

    public class DosTestScenarioTransformer
        : IDosTestScenarioTransformer
    {
        public string Name { get; set; }
        public object[] Arguments { get; set; }
    }
}
namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    public static class DosRequestMismatchResult
    {
        public static DosTestScenarioTransformer ServerError
        {
            get { return new DosTestScenarioTransformer { Name = "ServerError" }; }
        }
    }
}
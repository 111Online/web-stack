
namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    public static class ServicesTransformedTo
    {
        public static DosTestScenarioTransformer AtLeastOneCallback
        {
            get { return new DosTestScenarioTransformer { Name = "AtLeastOneCallback" }; }
        }

        public static DosTestScenarioTransformer EmptyServiceList
        {
            get { return new DosTestScenarioTransformer { Name = "EmptyServiceList" }; }
        }

        public static DosTestScenarioTransformer OnlyOneCallback
        {
            get { return new DosTestScenarioTransformer { Name = "OnlyOneCallback" }; }
        }

        public static DosTestScenarioTransformer ServerError
        {
            get { return new DosTestScenarioTransformer { Name = "ServerError" }; }
        }
    }
}
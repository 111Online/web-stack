namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System.Collections.Generic;
    using Models.Models.Business;

    public interface ITestScenario {
        string Postcode { get; }
    }

    public interface ITestScenario<T>
        : ITestScenario {
        ICollection<T> Requests { get; }
    }

    public interface IDosTestScenario
        : ITestScenario<DosTestScenarioRequest> {
        bool Matches(Postcode postcode);
    }

    public class DosTestScenario 
       : IDosTestScenario {

        public string Postcode { get; set; }

        public ICollection<DosTestScenarioRequest> Requests { get; set; }

        public bool Matches(Postcode postcode) {
            return new Postcode(this.Postcode).Equals(postcode);
        }

        public DosTestScenario() {
            Requests = new List<DosTestScenarioRequest>();
        }
    }
}
namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System.Collections.Generic;
    using Models.Models.Business;

    public interface IDosTestScenario {
        string Postcode { get; }
        string Description { get; }
        List<DosTestScenarioRequest> Requests { get; }
        bool Matches(Postcode postcode);
    }

    public class DosTestScenario 
       : IDosTestScenario {

        public string Postcode { get; set; }
        public string Description { get; set; }

        public List<DosTestScenarioRequest> Requests { get; set; }

        public bool Matches(Postcode postcode) {
            return new Postcode(this.Postcode).Equals(postcode);
        }

        public DosTestScenario() {
            Requests = new List<DosTestScenarioRequest>();
        }
    }
}
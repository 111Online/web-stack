namespace NHS111.DOS.Functional.Tests.TestBenchApi {
    using System.Collections.Generic;
    using Models.Models.Web.DosRequests;

    public class DosTestScenarioRequest {
        public DosFilteredCase InboundDosFilteredCase { get; set; }
        public DosFilteredCase OutboundDosFilteredCase { get; set; }
        public List<DosTestScenarioTransformer> MatchSequence { get; set; }

        /// <summary>
        /// A mismatch is when the inbound postcode matches but the rest of the InboundDosFilteredCase does not.
        /// </summary>
        public List<DosTestScenarioTransformer> MismatchSequence { get; set; }

        public DosTestScenarioRequest() {
            MatchSequence = new List<DosTestScenarioTransformer>();
            MismatchSequence = new List<DosTestScenarioTransformer>();
        }
    }
}
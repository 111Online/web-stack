using NHS111.Models.Models.Web.FromExternalServices;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Web.Presentation.Builders
{
    public class JourneyHistoryWrangler : IJourneyHistoryWrangler
    {
        public string GetPathwayNumbers(IList<JourneyStep> journeySteps)
        {
            return string.Join(",", GetPathwayNumbersList(journeySteps));
        }

        public IList<string> GetPathwayNumbersList(IList<JourneyStep> journeySteps)
        {
            List<string> pathways = journeySteps.Reverse()
                                                .Where(step => !string.IsNullOrEmpty(step.QuestionId))
                                                .Select(step => ConvertQuestionIdToPathwayId(step.QuestionId))
                                                .Distinct()
                                                .Reverse()
                                                .ToList();

            return pathways;
        }

        private static string ConvertQuestionIdToPathwayId(string questionId)
        {
            if (string.IsNullOrEmpty(questionId))
                return string.Empty;

            var array = questionId.Split('.');
            return array.Length > 0 ? array[0] : string.Empty;
        }
    }

    public interface IJourneyHistoryWrangler
    {
        string GetPathwayNumbers(IList<JourneyStep> journeySteps);
        IList<string> GetPathwayNumbersList(IList<JourneyStep> journeySteps);
    }
}

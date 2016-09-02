using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Web.Presentation.Builders
{
    public class SymptomGroupBuilder : ISymptomGroupBuilder
    {
        public string GetSymptomGroups(IList<JourneyStep> journeySteps)
        {
            List<string> pathways = journeySteps.Reverse()
                                                .Where(step => !string.IsNullOrEmpty(step.QuestionId))
                                                .Select(step => ConvertQuestionIdToPathwayId(step.QuestionId))
                                                .Distinct()
                                                .Reverse()
                                                .ToList();

            return string.Join(",", pathways);
        }

        private static string ConvertQuestionIdToPathwayId(string questionId)
        {
            if (string.IsNullOrEmpty(questionId))
                return string.Empty;
            
            var array = questionId.Split('.');
            return array.Length > 0 ? array[0] : string.Empty;
        }
    }

    public interface ISymptomGroupBuilder
    {
        string GetSymptomGroups(IList<JourneyStep> journeySteps);
    }
}

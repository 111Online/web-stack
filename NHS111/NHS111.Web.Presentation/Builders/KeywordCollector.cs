using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Web.Presentation.Builders
{
    public interface IKeywordCollector
    {
        JourneyViewModel Collect(Answer answer, JourneyViewModel exitingJourneyModel);
        IEnumerable<string> ParseKeywords(string keywordsString);
        IEnumerable<string> CollectFromJourneySteps(List<JourneyStep> journeySteps);
    }

    public class KeywordCollector : IKeywordCollector
    {
        public JourneyViewModel Collect(Answer answer, JourneyViewModel exitingJourneyModel)
        {
            var journeyViewModel = exitingJourneyModel;
            if (answer != null && !String.IsNullOrEmpty(answer.Keywords))
            {
                var keywordsToAdd = ParseKeywords(answer.Keywords).ToList();
                journeyViewModel.CollectedKeywords = journeyViewModel.CollectedKeywords.Union(keywordsToAdd).ToList();
            }
            return journeyViewModel;
        }

        public IEnumerable<string> ParseKeywords(string keywordsString) {
            if (string.IsNullOrEmpty(keywordsString))
                return new List<string>();

            var keywordsList = keywordsString.Split('|')
                .Select(k => k.Trim()).Where(k => !String.IsNullOrEmpty(k))
                .ToList();

            return keywordsList;
        }


        public IEnumerable<string> CollectFromJourneySteps(List<JourneyStep> journeySteps)
        {
           return journeySteps
               .Select(s => s.Answer)
               .SelectMany(a => ParseKeywords(a.Keywords)).Distinct().ToList();
        }
    }
}

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
        KeywordBag CollectFromJourneySteps(List<JourneyStep> journeySteps);
        IEnumerable<string> ConsolidateKeywords(KeywordBag keywordBag);
    }

    public class KeywordCollector : IKeywordCollector
    {
        public JourneyViewModel Collect(Answer answer, JourneyViewModel exitingJourneyModel)
        {
            var journeyViewModel = exitingJourneyModel;
            if (answer != null)
            {
                if (!String.IsNullOrEmpty(answer.Keywords))
                {
                    var keywordsToAdd = ParseKeywords(answer.Keywords).ToList();
                    journeyViewModel.CollectedKeywords.Keywords = journeyViewModel.CollectedKeywords.Keywords.Union(keywordsToAdd).ToList();
                }
                if (!String.IsNullOrEmpty(answer.ExcludeKeywords))
                {
                    var excludeKeywordsToAdd = ParseKeywords(answer.ExcludeKeywords).ToList();
                    journeyViewModel.CollectedKeywords.ExcludeKeywords =
                        journeyViewModel.CollectedKeywords.ExcludeKeywords.Union(excludeKeywordsToAdd).ToList();
                }
                
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


        public KeywordBag CollectFromJourneySteps(List<JourneyStep> journeySteps)
        {
           var keywords = journeySteps
               .Select(s => s.Answer)
               .SelectMany(a => ParseKeywords(a.Keywords)).Distinct().ToList();

           var excludekeywords = journeySteps
             .Select(s => s.Answer)
             .SelectMany(a => ParseKeywords(a.ExcludeKeywords)).Distinct().ToList();

            return new KeywordBag(keywords, excludekeywords);
        }


        public IEnumerable<string> ConsolidateKeywords(KeywordBag keywordBag)
        {
            return keywordBag.Keywords.Except(keywordBag.ExcludeKeywords);
        }
    }
}

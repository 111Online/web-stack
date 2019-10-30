using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Comparer;

namespace NHS111.Utils.Parser
{
    public interface IKeywordCollector
    {
        JourneyViewModel Collect(Answer answer, JourneyViewModel exitingJourneyModel);
        IEnumerable<Keyword> ParseKeywords(string keywordsString);
        IEnumerable<Keyword> ParseKeywords(string keywordsString, bool isFromAnswer);
        KeywordBag CollectKeywordsFromPreviousQuestion(KeywordBag keywordBag, List<JourneyStep> journeySteps);
        IEnumerable<string> ConsolidateKeywords(KeywordBag keywordBag);
        KeywordBag CollectKeywords(IEnumerable<string> keywords, IEnumerable<string> excludeKeywords);
    }

    public class KeywordCollector : IKeywordCollector
    {
        public JourneyViewModel Collect(Answer answer, JourneyViewModel existingJourneyModel)
        {
            var journeyViewModel = existingJourneyModel;
            if (answer != null)
            {
                if (!string.IsNullOrEmpty(answer.Keywords))
                {
                    var keywordsToAdd = ParseKeywords(answer.Keywords).ToList();
                    journeyViewModel.CollectedKeywords.Keywords = journeyViewModel.CollectedKeywords.Keywords.Union(keywordsToAdd, new KeywordComparer()).ToList();
                }
                if (!string.IsNullOrEmpty(answer.ExcludeKeywords))
                {
                    var excludeKeywordsToAdd = ParseKeywords(answer.ExcludeKeywords).ToList();
                    journeyViewModel.CollectedKeywords.ExcludeKeywords =
                        journeyViewModel.CollectedKeywords.ExcludeKeywords.Union(excludeKeywordsToAdd, new KeywordComparer()).ToList();
                }

            }
            return journeyViewModel;
        }

        public IEnumerable<Keyword> ParseKeywords(string keywordsString)
        {
            return ParseKeywords(keywordsString, true);
        }

        public IEnumerable<Keyword> ParseKeywords(string keywordsString, bool isFromAnswer)
        {
            if (string.IsNullOrEmpty(keywordsString))
                return new List<Keyword>();

            var keywordsList = keywordsString.Split('|')
                .Select(k => k.Trim())
                .Where(k => !string.IsNullOrEmpty(k))
                .Select(k => new Keyword() { Value = k, IsFromAnswer = isFromAnswer })
                .ToList();

            return keywordsList;
        }

        public KeywordBag CollectKeywords(IEnumerable<string> keywords, IEnumerable<string> excludeKeywords)
        {
            var bag = new KeywordBag();

            var kw = keywords
                .SelectMany(a => ParseKeywords(a, false)).Distinct(new KeywordComparer()).ToList();

            var exKw = excludeKeywords
                .SelectMany(a => ParseKeywords(a, false)).Distinct(new KeywordComparer()).ToList();

            bag.Keywords = bag.Keywords.Union(kw, new KeywordComparer()).ToList();
            bag.ExcludeKeywords = bag.ExcludeKeywords.Union(exKw, new KeywordComparer()).ToList();

            return bag;
        }

        public KeywordBag CollectKeywordsFromPreviousQuestion(KeywordBag keywordBag, List<JourneyStep> journeySteps)
        {
            var bag = CollectKeywordsFromNonJourneySteps(keywordBag);

            if (journeySteps == null) return bag;

            var keywords = journeySteps
                .Select(s => s.Answer)
                .SelectMany(a => ParseKeywords(a.Keywords)).Distinct(new KeywordComparer()).ToList();

            var excludeKeywords = journeySteps
                .Select(s => s.Answer)
                .SelectMany(a => ParseKeywords(a.ExcludeKeywords)).Distinct(new KeywordComparer()).ToList();

            bag.Keywords = bag.Keywords.Union(keywords, new KeywordComparer()).ToList();
            bag.ExcludeKeywords = bag.ExcludeKeywords.Union(excludeKeywords, new KeywordComparer()).ToList();

            return bag;
        }

        public IEnumerable<string> ConsolidateKeywords(KeywordBag keywordBag)
        {
            return keywordBag.Keywords.Except(keywordBag.ExcludeKeywords, new KeywordComparer()).Select(k => k.Value);
        }

        private KeywordBag CollectKeywordsFromNonJourneySteps(KeywordBag keywordBag)
        {
            if (keywordBag == null) return new KeywordBag();

            return new KeywordBag()
            {
                Keywords = keywordBag.Keywords.Where(k => !k.IsFromAnswer).ToList(),
                ExcludeKeywords = keywordBag.ExcludeKeywords.Where(k => !k.IsFromAnswer).ToList()
            };
        }
    }
}

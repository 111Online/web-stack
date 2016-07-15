using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NHS111.Business.Glossary.Api.Models;
using NHS111.Domain.Glossary;
using NHS111.Models.Models.Domain;
using DefinedTerm = NHS111.Models.Models.Domain.DefinedTerm;


namespace NHS111.Business.Glossary.Api.Services
{
    public class TermsService : ITermsService
    {
        private const string TERMS_CACHE_KEY = "DEFINEDTERMS";
        private char[] PUNCTUATION_EXEMPTIONS = new char[] {'-'};
        private IDefinitionRepository _definitionRepository;
        public TermsService(IDefinitionRepository definitionRepository)
        {
            _definitionRepository = definitionRepository;
        }

        public async Task<IEnumerable<DefinedTerm>> ListDefinedTerms()
        {
            return await Task.Run(() =>
            {
  
                if (MemoryCache.Cache.Exists(TERMS_CACHE_KEY))
                    return 
                        (IEnumerable<DefinedTerm>)
                            MemoryCache.Cache.Get(TERMS_CACHE_KEY);
   
                var terms = _definitionRepository.List();
                var synonymDefinitions = GetSynonymDefinitions(terms);

                var allterms =
                    MergeTermDefinitions(terms, synonymDefinitions).ToList();

                MemoryCache.Cache.Store(TERMS_CACHE_KEY, allterms);
                return allterms;
                
            });
        }

        public async Task<IEnumerable<DefinedTerm>> FindContainedTerms(string text)
        {
            var normalisedtext = AppendAndPrependSpaces(RemovePunctuation(text.ToLower()));
            var terms = await ListDefinedTerms();
            return terms.Where(t => normalisedtext.Contains(" " + t.Term.ToLower() + " "));
        }

        private string AppendAndPrependSpaces(string text)
        {
            return " " + text + " ";
        }

        private string RemovePunctuation(string text)
        {
            return new String(text.Where(c => !(!PUNCTUATION_EXEMPTIONS.Contains(c) && Char.IsPunctuation(c))).ToArray());
        }


        public IEnumerable<DefinedTerm> MergeTermDefinitions(IEnumerable<Models.DefinedTerm> terms, IEnumerable<DefinedTerm> synonymDefinitions)
        {
            return terms.Select(
                t => new DefinedTerm() {Term = t.Term, Definition = t.Definition})
                .Union(synonymDefinitions);
        }

        public IEnumerable<DefinedTerm> GetSynonymDefinitions(IEnumerable<Models.DefinedTerm> terms)
        {
            return terms.Where(t=> !String.IsNullOrEmpty(t.Synonyms)).SelectMany(
                t =>
                    t.Synonyms.Split('|')
                        .Select(
                            s => new DefinedTerm() {Term = s, Definition = t.Definition}));
        }
    }
}
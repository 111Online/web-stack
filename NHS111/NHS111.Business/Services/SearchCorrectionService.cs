using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Comparer;

namespace NHS111.Business.Services
{
    public class SearchCorrectionService : ISearchCorrectionService
    {
        private readonly IPathwayService _pathwayService;

        public SearchCorrectionService(IPathwayService pathwayService)
        {
            _pathwayService = pathwayService;
        }

        public async Task<string> GetCorrection(string input)
        {
            input = input.ToLower();
            var pathways = JsonConvert.DeserializeObject<List<GroupedPathways>>(await _pathwayService.GetPathways(true));
            
            var pathwaysMatches = pathways.Where(x => x.Group.ToLower().Contains(input)).ToList();
            if (!pathwaysMatches.Any())// && !correctedTerms.Any())
                return JsonConvert.SerializeObject(pathways);

            //pathwaysMatches.AddRange(correctedTerms);
            return JsonConvert.SerializeObject(pathwaysMatches.Distinct(new PathwaysComparer()).OrderByDescending(x=>x.Group));
        }
    }

    public interface ISearchCorrectionService
    {
        Task<string> GetCorrection(string input);
    }
}
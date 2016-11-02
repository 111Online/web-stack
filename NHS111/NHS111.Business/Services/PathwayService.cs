using System.Collections.Generic;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Utils.Helpers;

namespace NHS111.Business.Services
{
    public class PathwayService : IPathwayService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;

        public PathwayService(IConfiguration configuration, IRestfulHelper restfulHelper)
        {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
        }

        public async Task<string> GetPathways(bool grouped, bool startingOnly)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiPathwaysUrl(grouped, startingOnly));
        }

        public async Task<string> GetPathway(string pathwayId)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiPathwayUrl(pathwayId));
        }

        public async Task<string> GetSymptomGroup(string pathwayNumbers)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiPathwaySymptomGroup(pathwayNumbers));
        }

        public async Task<string> GetIdentifiedPathway(string pathwayNumbers, string gender, int age)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiIdentifiedPathwayUrl(pathwayNumbers, gender, age));
        }

        public async Task<string> GetIdentifiedPathwayFromTitle(string pathwayTitle, string gender, int age)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiIdentifiedPathwayFromTitleUrl(pathwayTitle, gender, age));
        }

        public async Task<string> GetPathwayNumbers(string pathwayTitle)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiPathwayNumbersUrl(pathwayTitle));
        }
    }

    public interface IPathwayService
    {
        Task<string> GetPathways(bool grouped, bool startingOnly);
        Task<string> GetPathway(string pathwayId);
        Task<string> GetSymptomGroup(string pathwayNumbers);
        Task<string> GetIdentifiedPathway(string pathwayNumbers, string gender, int age);
        Task<string> GetIdentifiedPathwayFromTitle(string pathwayTitle, string gender, int age);
        Task<string> GetPathwayNumbers(string pathwayTitle);
    }
}
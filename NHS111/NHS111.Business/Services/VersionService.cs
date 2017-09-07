using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Utils.Helpers;

namespace NHS111.Business.Services
{
    public class VersionService : IVersionService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;

        public VersionService(IConfiguration configuration, IRestfulHelper restfulHelper)
        {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
        }

        public async Task<string> GetVersionInfo()
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiVersionUrl());
        }
    }

    public interface IVersionService
    {
        Task<string> GetVersionInfo();
    }
}

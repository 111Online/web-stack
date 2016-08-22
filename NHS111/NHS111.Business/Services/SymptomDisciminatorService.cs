using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Utils.Helpers;

namespace NHS111.Business.Services
{

    public class SymptomDisciminatorService : ISymptomDisciminatorService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;

        public SymptomDisciminatorService(IConfiguration configuration, IRestfulHelper restfulHelper)
        {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
        }

        public async Task<string> GetSymptomDisciminator(string id)
        {
            var response = await _restfulHelper.GetResponseAsync(_configuration.GetDomainApiSymptomDisciminatorUrl(id));
            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured requesting {0}. {1}", _configuration.GetDomainApiSymptomDisciminatorUrl(id), await response.Content.ReadAsStringAsync()));
            return await response.Content.ReadAsStringAsync();
        }

    }

    public interface ISymptomDisciminatorService
    {
        Task<string> GetSymptomDisciminator(string id);
    }
}

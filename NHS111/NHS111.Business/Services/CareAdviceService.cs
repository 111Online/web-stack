
namespace NHS111.Business.Services {
    using System;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using NHS111.Business.Configuration;
    using NHS111.Utils.Helpers;
    using Newtonsoft.Json;
    using System.Text;

    public class CareAdviceService
        : ICareAdviceService {
        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;

        public CareAdviceService(IConfiguration configuration, IRestfulHelper restfulHelper) {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
        }

        public async Task<string> GetCareAdvice(int age, string gender, IEnumerable<string> markers) {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiCareAdviceUrl(age, gender, markers));
        }

        public async Task<string> GetCareAdvice(string ageCategory, string gender, string keywords, string dxCode) {
            var request = new HttpRequestMessage {
                Content = new StringContent(JsonConvert.SerializeObject(keywords), Encoding.UTF8, "application/json")
            };
            var domainApiCareAdviceUrl = _configuration.GetDomainApiCareAdviceUrl(dxCode, ageCategory, gender);
            var response = await _restfulHelper.PostAsync(domainApiCareAdviceUrl, request);

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured requesting {0}. {1}", domainApiCareAdviceUrl, await response.Content.ReadAsStringAsync()));

            return await response.Content.ReadAsStringAsync();
        }
    }

    public interface ICareAdviceService {
        Task<string> GetCareAdvice(int age, string gender, IEnumerable<string> markers);
        Task<string> GetCareAdvice(string ageCategory, string gender, string keywords, string dxCode);
    }
}

namespace NHS111.Web.Controllers {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models.Models.Domain;
    using Models.Models.Web;
    using Newtonsoft.Json;
    using Presentation.Configuration;
    using Utils.Helpers;

    public class TopicController
        : Controller {

        public TopicController(IRestfulHelper restfulHelper, IConfiguration configuration) {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<ActionResult> Search(string q, string gender, int age) {
            var ageGroup = new AgeCategory(age);
            var response = await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwaySearchUrl(gender, ageGroup.Value));
            var results = JsonConvert.DeserializeObject<List<SearchResultViewModel>>(response);
            return View(new SearchJourneyViewModel { Results = results });
        }

        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.Helpers;
using NHS111.Utils.Parser;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Presentation.Builders
{
    public class SurveyLinkViewModelBuilder : ISurveyLinkViewModelBuilder
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public SurveyLinkViewModelBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<SurveyLinkViewModel> SurveyLinkBuilder(OutcomeViewModel model)
        {
            var jsonParser = new JourneyJsonParser(model.JourneyJson);
            var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayIdUrl(jsonParser.LastPathwayNo, model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age);
            var response = await _restfulHelper.GetAsync(businessApiPathwayUrl);
            var pathway = JsonConvert.DeserializeObject<Pathway>(response);

            return new SurveyLinkViewModel()
            {
                DispositionCode = model.Id,
                DispositionDateTime = model.DispositionTime,
                EndPathwayNo = (pathway != null) ? pathway.PathwayNo : string.Empty,
                EndPathwayTitle = (pathway != null) ? pathway.Title : string.Empty,
                JourneyId = model.JourneyId.ToString(),
                PathwayNo = model.PathwayNo,
                DigitalTitle = model.DigitalTitle
            };
        }
    }

    public interface ISurveyLinkViewModelBuilder
    {
        Task<SurveyLinkViewModel> SurveyLinkBuilder(OutcomeViewModel model);
    }
}

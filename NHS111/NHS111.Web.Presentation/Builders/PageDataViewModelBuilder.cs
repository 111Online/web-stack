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
    public class PageDataViewModelBuilder : IPageDataViewModelBuilder
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public PageDataViewModelBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<PageDataViewModel> PageDataBuilder(PageDataViewModel model)
        {
            model.Date = DateTime.Now.Date.ToShortDateString();
            model.Time = DateTime.Now.ToShortTimeString();
            
            Pathway currentPathway = null;
            if (!string.IsNullOrEmpty(model.QuestionId) && model.QuestionId.Contains("."))
            {
                var currentPathwayNo = model.QuestionId.Split('.')[0];
                if (!currentPathwayNo.Equals(model.StartingPathwayNo))
                {
                    var businessApiPathwayUrl =
                        _configuration.GetBusinessApiPathwayIdUrl(currentPathwayNo, model.Gender, new AgeCategory(model.Age).MinimumAge);
                    var response = await _restfulHelper.GetAsync(businessApiPathwayUrl);
                    currentPathway = JsonConvert.DeserializeObject<Pathway>(response);
                }
            }
            model.PathwayNo = (currentPathway != null) ? currentPathway.PathwayNo : string.Empty;
            model.PathwayTitle = (currentPathway != null) ? currentPathway.Title : string.Empty;

            return model;
        }
    }

    public interface IPageDataViewModelBuilder
    {
        Task<PageDataViewModel> PageDataBuilder(PageDataViewModel model);
    }
}

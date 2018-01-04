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

        public async Task<PageDataViewModel> PageDataBuilder(JourneyViewModel model, string campaign, string source)
        {
            Pathway currentPathway = null;
            if (!string.IsNullOrEmpty(model.Id) && model.Id.Contains("."))
            {
                var currentPathwayNo = model.Id.Split('.')[0];
                var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayIdUrl(currentPathwayNo, model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age);
                var response = await _restfulHelper.GetAsync(businessApiPathwayUrl);
                currentPathway = JsonConvert.DeserializeObject<Pathway>(response);
            }

            return new PageDataViewModel()
            {
                Page = model.PageData.Page,
                Campaign = campaign,
                Source = source,
                QuestionId = model.Id,
                TxNumber = model.QuestionNo,
                StartingPathwayNo = model.PathwayNo,
                StartingPathwayTitle = model.PathwayTitle,
                PathwayNo = (currentPathway != null) ? currentPathway.PathwayNo : string.Empty,
                PathwayTitle = (currentPathway != null) ? currentPathway.Title : string.Empty,
                Gender = model.UserInfo.Demography.Gender,
                Age = new AgeCategory(model.UserInfo.Demography.Age).Value,
                SearchString = model.EntrySearchTerm,
                DxCode = model.Id
            };
        }
    }

    public interface IPageDataViewModelBuilder
    {
        Task<PageDataViewModel> PageDataBuilder(JourneyViewModel model, string campaign, string source);
    }
}

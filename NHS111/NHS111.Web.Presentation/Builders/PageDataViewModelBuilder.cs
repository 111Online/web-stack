using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace NHS111.Web.Presentation.Builders
{
    public class PageDataViewModelBuilder : BaseBuilder, IPageDataViewModelBuilder
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public PageDataViewModelBuilder(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<PageDataViewModel> PageDataBuilder(PageDataViewModel model)
        {
            model.Date = DateTime.Now.Date.ToShortDateString();
            model.Time = DateTime.Now.ToString("HH:mm:ss");

            Pathway currentPathway = null;
            if (!string.IsNullOrEmpty(model.QuestionId) && model.QuestionId.Contains("."))
            {
                var currentPathwayNo = model.QuestionId.Split('.')[0];
                if (!currentPathwayNo.Equals(model.StartingPathwayNo))
                {
                    var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayIdUrl(currentPathwayNo, model.Gender, new AgeCategory(model.Age).MinimumAge);
                    var response = await _restClient.ExecuteTaskAsync<Pathway>(new JsonRestRequest(businessApiPathwayUrl, Method.GET));

                    CheckResponse(response);

                    currentPathway = response.Data;
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

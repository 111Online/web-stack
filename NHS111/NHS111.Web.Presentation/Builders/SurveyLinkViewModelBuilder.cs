using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.Helpers;
using NHS111.Utils.Parser;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    using NHS111.Models.Models.Web.FromExternalServices;
    using StructureMap.Query;

    public class SurveyLinkViewModelBuilder : BaseBuilder, ISurveyLinkViewModelBuilder
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public SurveyLinkViewModelBuilder(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<SurveyLinkViewModel> SurveyLinkBuilder(OutcomeViewModel model)
        {
            var jsonParser = new JourneyJsonParser(model.JourneyJson);
            var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayIdUrl(jsonParser.LastPathwayNo, model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age);
            var response = await _restClient.ExecuteTaskAsync<Pathway>(new JsonRestRequest(businessApiPathwayUrl, Method.GET));

            CheckResponse(response);

            var pathway = response.Data;
            var resultingDxCode = model.Is999Callback ? FromOutcomeViewModelToDosViewModel.DispositionResolver.Remap(model.Id) : model.Id;
            var result = new SurveyLinkViewModel()
            {
                DispositionCode = model.Id,
                DispositionDateTime = model.DispositionTime,
                EndPathwayNo = (pathway != null) ? pathway.PathwayNo : string.Empty,
                EndPathwayTitle = (pathway != null) ? pathway.Title : string.Empty,
                JourneyId = model.JourneyId.ToString(),
                PathwayNo = model.PathwayNo,
                DigitalTitle = model.DigitalTitle,
                Campaign = model.Campaign,
                CampaignSource = model.Source,
                ValidationCallbackOffered = model.Is999Callback || model.IsEDCallback
            };

            AddServiceInformation(model, result);

            return result;
        }

        public void AddServiceInformation(OutcomeViewModel model, SurveyLinkViewModel surveyLinkViewModel) {
            var serviceOptions = new List<OnlineDOSServiceType>();
            var services = new List<ServiceViewModel>();
            if (model.GroupedDosServices != null)
            {
                services = model.GroupedDosServices.SelectMany(g => g.Services).ToList();
                serviceOptions = services.GroupBy(s => s.OnlineDOSServiceType).Select(s => s.Key).ToList();
            }

            surveyLinkViewModel.ServiceCount = services.Count;
            surveyLinkViewModel.ServiceOptions = string.Join(",", serviceOptions);
        }
    }

    public interface ISurveyLinkViewModelBuilder
    {
        Task<SurveyLinkViewModel> SurveyLinkBuilder(OutcomeViewModel model);
        void AddServiceInformation(OutcomeViewModel model, SurveyLinkViewModel surveyLinkViewModel);
    }
}

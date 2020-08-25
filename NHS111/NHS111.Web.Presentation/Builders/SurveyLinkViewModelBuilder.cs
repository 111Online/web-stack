using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using NHS111.Features;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Business.MicroSurvey;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Parsers;
using NHS111.Utils.RestTools;
using RestSharp;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{

    public class SurveyLinkViewModelBuilder : BaseBuilder, ISurveyLinkViewModelBuilder
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly ISurveyLinkFeature _surveyLinkFeature;

        public SurveyLinkViewModelBuilder(ILoggingRestClient restClient, IConfiguration configuration, ISurveyLinkFeature surveyLinkFeature)
        {
            _restClient = restClient;
            _configuration = configuration;
            _surveyLinkFeature = surveyLinkFeature;
        }

        public async Task<SurveyLinkViewModel> SurveyLinkBuilder(OutcomeViewModel model)
        {
            var jsonParser = new JourneyJsonParser(model.JourneyJson);
            var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayIdUrl(jsonParser.LastPathwayNo, model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age);
            var response = await _restClient.ExecuteAsync<Pathway>(new JsonRestRequest(businessApiPathwayUrl, Method.GET));

            CheckResponse(response);

            var pathway = response.Data;
            var resultingDxCode = model.Is999Callback ? FromOutcomeViewModelToDosViewModel.DispositionResolver.Remap(model.Id) : model.Id;
            var result = new SurveyLinkViewModel
            {
                DispositionCode = model.Id,
                DispositionDateTime = model.DispositionTime,
                EndPathwayNo = (pathway != null) ? pathway.PathwayNo : string.Empty,
                EndPathwayTitle = (pathway != null) ? pathway.Title : string.Empty,
                JourneyId = model.JourneyId.ToString(),
                SessionId = model.SessionId,
                PathwayNo = model.PathwayNo,
                DigitalTitle = model.DigitalTitle,
                Campaign = model.Campaign,
                CampaignSource = model.Source,
                ValidationCallbackOffered = model.HasAcceptedCallbackOffer.HasValue,
                GuidedSelection = GetGuidedSelectionParameterFrom(model),
                RecommendedServiceTypeAlias = GetServiceTypeAliasParameterFrom(model),
                StartUrl = model.StartParameter
            };

            var isPharmacyPathway = result.EndPathwayNo == "PW1827";
            result.SurveyId = isPharmacyPathway ? _surveyLinkFeature.PharmacySurveyId : _surveyLinkFeature.SurveyId;
            AddServiceInformation(model, result);

            model.SurveyLink = result;
            AddEmbeddedDataInformation(model, result);            

            return result;
        }

        private void AddEmbeddedDataInformation(OutcomeViewModel model, SurveyLinkViewModel surveyLinkViewModel)
        {
            var embeddedData = Mapper.Map<EmbeddedData>(model);
            surveyLinkViewModel.EmbeddedData = embeddedData;
        }

        public void AddServiceInformation(OutcomeViewModel model, SurveyLinkViewModel surveyLinkViewModel)
        {
            var serviceOptions = new List<string>();
            var services = new List<ServiceViewModel>();

            if (model.GroupedDosServices != null)
            {
                services = model.GroupedDosServices.SelectMany(g => g.Services).ToList();
                serviceOptions = services.GroupBy(s => s.OnlineDOSServiceType.Id).Select(s => s.Key).ToList();
            }

            surveyLinkViewModel.Services = new List<ServiceViewModel>();

            //For covid specific outcomes, only set these params if the results contain itk services
            if (OutcomeGroup.Isolate111.Equals(model.OutcomeGroup))
            {
                if(model.DosCheckCapacitySummaryResult.HasITKServices)
                {
                    surveyLinkViewModel.ServiceCount = services.Count;
                    surveyLinkViewModel.ServiceOptions = string.Join(",", serviceOptions);
                }
            }
            else
            {
                surveyLinkViewModel.ServiceCount = services.Count;
                surveyLinkViewModel.ServiceOptions = string.Join(",", serviceOptions);
            }
            
            if (!model.DosCheckCapacitySummaryResult.ResultListEmpty)
            {
                var recommendedService = model.DosCheckCapacitySummaryResult.Success.Services.First();
                surveyLinkViewModel.RecommendedServiceId = recommendedService.Id;
                surveyLinkViewModel.RecommendedServiceType = recommendedService.OnlineDOSServiceType.Id;
                surveyLinkViewModel.RecommendedServiceName = recommendedService.PublicName;
                
                var otherServices = model.DosCheckCapacitySummaryResult.Success.Services.Skip(1).ToList();
                services = model.DosCheckCapacitySummaryResult.Success.Services;
                serviceOptions = services.GroupBy(s => s.OnlineDOSServiceType.Id).Select(s => s.Key).ToList();
            }
            
            surveyLinkViewModel.ServiceCount = services.Count;
            surveyLinkViewModel.ServiceOptions = string.Join(",", serviceOptions);

            var serviceTypeId = model.SelectedService != null ? model.SelectedService.ServiceType.Id : -1;
            surveyLinkViewModel.BookPharmacyCall = BookPharmacyCallModelBuilder.BookPharmacyCallValue(model.Id, serviceTypeId, services, OutcomeGroup.PrePopulatedDosResultsOutcomeGroups.Contains(model.OutcomeGroup));
            surveyLinkViewModel.RecommendedServiceTypeAlias = GetServiceTypeAliasParameterFrom(model);
        }

        public void AddDispositionReason(string reason, SurveyLinkViewModel surveyLinkViewModel)
        {
            surveyLinkViewModel.DispositionChoiceReasoning = reason;
        }
        private string GetGuidedSelectionParameterFrom(OutcomeViewModel model)
        {
            return model.HasBeenViaGuidedSelection
                ? model.IsViaGuidedSelection ? "true" : "false"
                : string.Empty;
        }
        private string GetServiceTypeAliasParameterFrom(OutcomeViewModel model)
        {
            if (!model.OutcomeGroup.IsServiceFirst) return string.Empty;
            if(model.DosCheckCapacitySummaryResult.ResultListEmpty) return "no-results";
            var firstService = model.DosCheckCapacitySummaryResult.Success.FirstService;
            var recommendedService = Mapper.Map<RecommendedServiceViewModel>(firstService);
            return recommendedService.IsCallbackServiceNotOfferingCallback ? string.Empty : firstService.ServiceTypeAlias;
        }
    }

    public interface ISurveyLinkViewModelBuilder
    {
        Task<SurveyLinkViewModel> SurveyLinkBuilder(OutcomeViewModel model);
        void AddServiceInformation(OutcomeViewModel model, SurveyLinkViewModel surveyLinkViewModel);
        void AddDispositionReason(string reason, SurveyLinkViewModel surveyLinkViewModel);
    }
}

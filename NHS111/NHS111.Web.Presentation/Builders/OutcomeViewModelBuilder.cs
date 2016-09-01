using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Logging;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class OutcomeViewModelBuilder : IOutcomeViewModelBuilder
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly ICacheManager<string, string> _cacheManager;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IAddressViewModelBuilder _addressViewModelBuilder;

        public OutcomeViewModelBuilder(ICareAdviceBuilder careAdviceBuilder, IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, ICacheManager<string, string> cacheManager, IKeywordCollector keywordCollector,
            IAddressViewModelBuilder addressViewModelBuilder)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _cacheManager = cacheManager;
            _keywordCollector = keywordCollector;
            _addressViewModelBuilder = addressViewModelBuilder;
        }

        public async Task<List<AddressInfo>> SearchPostcodeBuilder(string input)
        {
            input = HttpUtility.UrlDecode(input);
            var listPaf = JsonConvert.DeserializeObject<List<PAF>>(await _restfulHelper.GetAsync(string.Format(_configuration.PostcodeSearchByIdApiUrl, input)));
            return _mappingEngine.Mapper.Map<List<AddressInfo>>(listPaf);
        }

        public async Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model, string lastQuestionAskedId)
        {
            if (OutcomeGroup.Call999.Equals(model.OutcomeGroup))
            {
                model.AddressSearchViewModel = _addressViewModelBuilder.Build(model);
                model.CareAdviceMarkers = model.State.Keys.Where(key => key.StartsWith("Cx"));
            }

            if (!String.IsNullOrEmpty(model.SymptomDiscriminatorCode))
            {
                model.SymptomDiscriminator = await GetSymptomDiscriminator(model.SymptomDiscriminatorCode);
            }

            if (!string.IsNullOrEmpty(lastQuestionAskedId))
            {
                model.SymptomGroup = await GetSymptomGroup(ConvertQuestionIdToPathwayId(lastQuestionAskedId));
            }

            model.UserId = Guid.NewGuid();
            model.WorseningCareAdvice = await _careAdviceBuilder.FillWorseningCareAdvice(model.UserInfo.Age,
                model.UserInfo.Gender);
            model.CareAdvices = await
                    _careAdviceBuilder.FillCareAdviceBuilder(model.Id, new AgeCategory(model.UserInfo.Age).Value, model.UserInfo.Gender,
                        _keywordCollector.ConsolidateKeywords(model.CollectedKeywords).ToList());
            return model;
        }

        private async Task<SymptomDiscriminator> GetSymptomDiscriminator(string symptomDiscriminatorCode)
        {

            var symptomDiscriminatorResponse = await
                _restfulHelper.GetResponseAsync(
                    string.Format(_configuration.GetBusinessApiSymptomDiscriminatorUrl(symptomDiscriminatorCode)));
            if (!symptomDiscriminatorResponse.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured getting the symptom discriminator at {0}. {1}",
                    _configuration.GetBusinessApiSymptomDiscriminatorUrl(symptomDiscriminatorCode),
                    await symptomDiscriminatorResponse.Content.ReadAsStringAsync()));

            return 
                JsonConvert.DeserializeObject<SymptomDiscriminator>(await symptomDiscriminatorResponse.Content.ReadAsStringAsync());
        }

        private static string ConvertQuestionIdToPathwayId(string questionId)
        {
            var array = questionId.Split('.');
            return array.Length > 0 ? array[0] : string.Empty;
        }

        private async Task<string> GetSymptomGroup(string pathway)
        {
            RestfulHelper restfulHelper = new RestfulHelper();

            var symptomGroupResponse = await
                restfulHelper.GetResponseAsync(string.Format(_configuration.GetBusinessApiPathwaySymptomGroupUrl(pathway)));
            if (!symptomGroupResponse.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured getting the symptom group for {0}.", pathway));

            return
                await symptomGroupResponse.Content.ReadAsStringAsync();
        }

        public async Task<OutcomeViewModel> ItkResponseBuilder(OutcomeViewModel model)
        {
            var itkRequestData = CreateItkDispatchRequest(model);
            var response = await SendItkMessage(itkRequestData);
            if (response.IsSuccessStatusCode)
            {
                model.ItkSendSuccess = true;
                var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            }
            else
            {
                model.ItkSendSuccess = false;
                Log4Net.Error("Error sending ITK message : Status Code -" + response.StatusCode.ToString() +
                              " Content -" + response.Content.ReadAsStringAsync());
            }
            model.CareAdvices =
                await
                    _careAdviceBuilder.FillCareAdviceBuilder(model.Id, new AgeCategory(model.UserInfo.Age).Value, model.UserInfo.Gender,
                        _keywordCollector.ConsolidateKeywords(model.CollectedKeywords).ToList());

            model.WorseningCareAdvice =
                await _careAdviceBuilder.FillWorseningCareAdvice(model.UserInfo.Age, model.UserInfo.Gender);
            return model;
        }

        private async Task<HttpResponseMessage> SendItkMessage(ITKDispatchRequest itkRequestData)
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(itkRequestData), Encoding.UTF8, "application/json")
            };
            var response = await _restfulHelper.PostAsync(_configuration.ItkDispatchApiUrl, request);
            return response;
        }

        private Authentication getItkAuthentication()
        {
            return new Authentication { UserName = ConfigurationManager.AppSettings["itk_credential_user"], Password = ConfigurationManager.AppSettings["itk_credential_password"] };
        }

        private ITKDispatchRequest CreateItkDispatchRequest(OutcomeViewModel model)
        {
            var itkRequestData = _mappingEngine.Mapper.Map<OutcomeViewModel, ITKDispatchRequest>(model);
            itkRequestData.Authentication = getItkAuthentication();
            return itkRequestData;
        }

        public async Task<OutcomeViewModel> PostCodeSearchBuilder(OutcomeViewModel model)
        {
            var addresses = await SearchPostcodeBuilder(model.AddressSearchViewModel.PostCode);
            model.AddressSearchViewModel.AddressInfoList = addresses;
            model.AddressSearchViewModel.PostcodeApiAddress = _configuration.PostcodeSearchByIdApiUrl;
            model.AddressSearchViewModel.PostcodeApiSubscriptionKey = _configuration.PostcodeSubscriptionKey;
            return model;
        }

        public async Task<OutcomeViewModel> PersonalDetailsBuilder(OutcomeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.AddressSearchViewModel.PostCode))
            {
                return await PostCodeSearchBuilder(model);
            }

            model.AddressSearchViewModel.PostcodeApiAddress = _configuration.PostcodeSearchByIdApiUrl;
            model.AddressSearchViewModel.PostcodeApiSubscriptionKey = _configuration.PostcodeSubscriptionKey;

            model.CareAdvices = await _careAdviceBuilder.FillCareAdviceBuilder(model.UserInfo.Age, model.UserInfo.Gender, model.CareAdviceMarkers.ToList());
            return model;
        }

    }

    public interface IOutcomeViewModelBuilder
    {
        Task<List<AddressInfo>> SearchPostcodeBuilder(string input);
        Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model, string lastQuestionAnsweredId);
        Task<OutcomeViewModel> PostCodeSearchBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PersonalDetailsBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> ItkResponseBuilder(OutcomeViewModel model);
    }
}
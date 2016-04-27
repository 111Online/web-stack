using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Itk;
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

        public OutcomeViewModelBuilder(ICareAdviceBuilder careAdviceBuilder, IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, ICacheManager<string, string> cacheManager)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _cacheManager = cacheManager;
        }

        public async Task<OutcomeViewModel> SearchSurgeryBuilder(string input)
        {
            var model = new OutcomeViewModel();
            if (!string.IsNullOrEmpty(input))
            {
                var surgeriers = JsonConvert.DeserializeObject<List<Surgery>>(await _restfulHelper.GetAsync(string.Format(_configuration.GPSearchByIdUrl, input)));
                model.SurgeryViewModel.Surgeries = surgeriers;
            }

            return model;
        }

        public async Task<List<AddressInfo>> SearchPostcodeBuilder(string input)
        {
            input = HttpUtility.UrlDecode(input);
            var listPaf = JsonConvert.DeserializeObject<List<PAF>>(await _restfulHelper.GetAsync(string.Format(_configuration.PostcodeSearchByIdApiUrl, input)));
            return _mappingEngine.Map<List<AddressInfo>>(listPaf);
        }

        public async Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model)
        {
            model.UserId = Guid.NewGuid();
            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            //var itkMessage = new ItkMessageBuilder(_cacheManager).WithExample().SetSummaryItems(
            //    journey.Steps.Select(a => new ItkMessageBuilder.SummaryItem(a.QuestionNo, a.QuestionTitle, a.Answer.Title))
            //    )
            //    .SetGender(model.UserInfo.Gender)
            //    .SetDateOfBirth(DateTime.Now.AddYears(-model.UserInfo.Age).ToShortDateString())
            //    .SetDispositionCode(model.Id.Replace("Dx", string.Empty))
            //    .SetProvider("111Digital")
            //    .SetInformantType("NotSpecified")
            //    .SetSendToRepeatCaller(false)
            //    .Build(model.UserId.ToString());

            return await AddCareAdvice(model, journey);
        }

        private async Task<OutcomeViewModel> AddCareAdvice(OutcomeViewModel model, Journey journey)
        {
            model.CareAdvices =
                await
                    _careAdviceBuilder.FillCareAdviceBuilder(model.UserInfo.Age, model.UserInfo.Gender,
                        model.CareAdviceMarkers.ToList());
            model.SymptomGroup = await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwaySymptomGroupUrl(
                string.Join(",", journey.Steps.Select(s => s.QuestionId.Split('.').First()).Distinct())));

            return model;
        }

        public async Task<OutcomeViewModel> ItkResponseBuilder(OutcomeViewModel model)
        {
            var itkRequestData = CreateItkDispatchRequest(model);
            var response = await SendItkMessage(itkRequestData);
            if (response.IsSuccessStatusCode)
            {
                model.ItkSendSuccess = true;
                var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
                model = await AddCareAdvice(model, journey);
            }
            else
            {
                model.ItkSendSuccess = false;
                Log4Net.Error("Error sending ITK message : Status Code -" + response.StatusCode.ToString() +
                              " Content -" + response.Content.ReadAsStringAsync());
            }
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

        private ITKDispatchRequest CreateItkDispatchRequest(OutcomeViewModel model)
        {
            var auth = new Authentication() {UserName = "admn", Password = "admnUat"};
            var itkRequestData = _mappingEngine.Map<OutcomeViewModel, ITKDispatchRequest>(model);
            itkRequestData.Authentication = auth;
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
        Task<OutcomeViewModel> SearchSurgeryBuilder(string input);
        Task<List<AddressInfo>> SearchPostcodeBuilder(string input);
        Task<OutcomeViewModel> DispositionBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PostCodeSearchBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> PersonalDetailsBuilder(OutcomeViewModel model);
        Task<OutcomeViewModel> ItkResponseBuilder(OutcomeViewModel model);
    }
}
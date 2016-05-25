using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Notifier;
using NHS111.Web.Presentation.Models;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class DOSBuilder : IDOSBuilder
    {
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly ICacheManager<string, string> _cacheManager;
        private readonly INotifier<string> _notifier;

        public DOSBuilder(ICareAdviceBuilder careAdviceBuilder, IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, ICacheManager<string, string> cacheManager, INotifier<string> notifier)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _cacheManager = cacheManager;
            _notifier = notifier;
        }

        public async Task<DosViewModel> DosResultsBuilder(OutcomeViewModel outcomeViewModel)
        {
            var model = _mappingEngine.Mapper.Map<DosViewModel>(outcomeViewModel);
            var surgery = await GetSelectedSurgery(model);

            var capacitySummaryRequest = await BuildCheckCapacitySummaryRequest(outcomeViewModel, surgery);
            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(capacitySummaryRequest), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.BusinessDosCheckCapacitySummaryUrl, request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var val = await response.Content.ReadAsStringAsync();
                model.CheckCapacitySummaryResultListJson = HttpUtility.HtmlDecode(val);
                var jObj = (JObject)JsonConvert.DeserializeObject(val);
                var result = jObj["CheckCapacitySummaryResult"];
                model.CheckCapacitySummaryResultList = result.ToObject<CheckCapacitySummaryResult[]>();
            }
            else
            {
                model.CheckCapacitySummaryResultList = new CheckCapacitySummaryResult[0];
            }

            return model;
        }

        public async Task<DosCheckCapacitySummaryRequest> BuildCheckCapacitySummaryRequest(
            OutcomeViewModel outcomeViewModel, Surgery surgery)
        {
            outcomeViewModel = await BuildSymptomGroup(outcomeViewModel);

            var dosCase = _mappingEngine.Mapper.Map<OutcomeViewModel, DosCase>(outcomeViewModel);
            dosCase.Surgery = surgery.SurgeryId;
            return new DosCheckCapacitySummaryRequest(_configuration.DosUsername, _configuration.DosPassword, dosCase);
        }

        private async Task<OutcomeViewModel> BuildSymptomGroup(OutcomeViewModel model)
        {
            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            model.SymptomGroup = await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwaySymptomGroupUrl(
                string.Join(",", journey.Steps.Select(s => s.QuestionId.Split('.').First()).Distinct())));

            return model;
        }


        private async Task<Surgery> GetSelectedSurgery(DosViewModel model)
        {
            var surgery = new Surgery();
            if (!string.IsNullOrEmpty(model.Surgery))
                surgery =
                    JsonConvert.DeserializeObject<Surgery>(
                        await _restfulHelper.GetAsync(string.Format(_configuration.GPSearchApiUrl, model.Surgery)));
            else
                surgery.SurgeryId = "UKN";
            return surgery;
        }

        public async Task<DosViewModel> FillServiceDetailsBuilder(DosViewModel model)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(model.CheckCapacitySummaryResultListJson);
            model.CheckCapacitySummaryResultList = jObj["CheckCapacitySummaryResult"].ToObject<CheckCapacitySummaryResult[]>();
            var selectedService = model.SelectedService;

            var itkMessage = await _cacheManager.Read(model.UserId.ToString());
            var document = XDocument.Parse(itkMessage);

            var serviceDetials = document.Root.Descendants("ServiceDetails").FirstOrDefault();
            serviceDetials.Element("id").SetValue(selectedService.IdField.ToString());
            serviceDetials.Element("name").SetValue(selectedService.NameField.ToString());
            serviceDetials.Element("odsCode").SetValue(selectedService.OdsCodeField.ToString());
            serviceDetials.Element("contactDetails").SetValue(selectedService.ContactDetailsField ?? "");
            serviceDetials.Element("address").SetValue(selectedService.AddressField.ToString());
            serviceDetials.Element("postcode").SetValue(selectedService.PostcodeField.ToString());

            _cacheManager.Set(model.UserId.ToString(), document.ToString());
            _notifier.Notify(_configuration.IntegrationApiItkDispatcher, model.UserId.ToString());

            model.CheckCapacitySummaryResultList = new CheckCapacitySummaryResult[] { selectedService };
            model.CareAdvices = await _careAdviceBuilder.FillCareAdviceBuilder(Convert.ToInt32(model.Age), model.Gender.ToString(), model.CareAdviceMarkers.ToList());

            return model;
        }
    }

    public interface IDOSBuilder
    {
        Task<DosViewModel> DosResultsBuilder(OutcomeViewModel outcomeViewModel);
        Task<DosViewModel> FillServiceDetailsBuilder(DosViewModel model);
    }
}
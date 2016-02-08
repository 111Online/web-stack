using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using StructureMap.Query;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class JustToBeSafeFirstViewModelBuilder : IJustToBeSafeFirstViewModelBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IRestfulHelper _restfulHelper;

        public JustToBeSafeFirstViewModelBuilder(IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
        }

        public async Task<Tuple<string, JourneyViewModel>> JustToBeSafeFirstBuilder(JustToBeSafeViewModel model)
        {
            var questionsJson = await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiJustToBeSafePartOneUrl, model.PathwayId));
            var questionsWithAnswers = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(questionsJson);
            if (!questionsWithAnswers.Any())
            {
                var journeyViewModel = new JourneyViewModel
                {
                    PathwayId = model.PathwayId,
                    PathwayNo = model.PathwayNo,
                    PathwayTitle = model.PathwayTitle,
                    UserInfo = model.UserInfo,
                    JourneyJson = model.JourneyJson,
                    State = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.StateJson),
                    StateJson = model.StateJson
                };
                var question = JsonConvert.DeserializeObject<QuestionWithAnswers>(await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiFirstQuestionUrl, model.PathwayId, model.StateJson)));
                _mappingEngine.Map(question, journeyViewModel);
                return new Tuple<string, JourneyViewModel>("../Question/Question", journeyViewModel);
            }
            model.Part = 1;
            model.Questions = questionsWithAnswers;
            model.QuestionsJson = questionsJson;
            model.JourneyJson = string.IsNullOrEmpty(model.JourneyJson) ? JsonConvert.SerializeObject(new Journey()) : model.JourneyJson;
            return new Tuple<string, JourneyViewModel>("../JustToBeSafe/JustToBeSafe", model);

        }
    }

    public interface IJustToBeSafeFirstViewModelBuilder
    {
        Task<Tuple<string, JourneyViewModel>> JustToBeSafeFirstBuilder(JustToBeSafeViewModel model);
    }
}
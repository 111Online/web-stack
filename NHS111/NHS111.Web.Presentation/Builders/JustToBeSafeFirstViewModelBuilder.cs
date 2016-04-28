﻿using System;
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
            var identifiedModel = await BuildIdentifiedModel(model);
            var questionsJson = await _restfulHelper.GetAsync(_configuration.GetBusinessApiJustToBeSafePartOneUrl(identifiedModel.PathwayId));
            var questionsWithAnswers = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(questionsJson);
            if (!questionsWithAnswers.Any())
            {
                var journeyViewModel = new JourneyViewModel
                {
                    PathwayId = identifiedModel.PathwayId,
                    PathwayNo = identifiedModel.PathwayNo,
                    PathwayTitle = identifiedModel.PathwayTitle,
                    UserInfo = identifiedModel.UserInfo,
                    JourneyJson = identifiedModel.JourneyJson,
                    State = JsonConvert.DeserializeObject<Dictionary<string, string>>(identifiedModel.StateJson),
                    StateJson = identifiedModel.StateJson
                };
                var question = JsonConvert.DeserializeObject<QuestionWithAnswers>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiFirstQuestionUrl(identifiedModel.PathwayId, identifiedModel.StateJson)));
                _mappingEngine.Map(question, journeyViewModel);
                return new Tuple<string, JourneyViewModel>("../Question/Question", journeyViewModel);
            }
            identifiedModel.Part = 1;
            identifiedModel.Questions = questionsWithAnswers;
            identifiedModel.QuestionsJson = questionsJson;
            identifiedModel.JourneyJson = string.IsNullOrEmpty(identifiedModel.JourneyJson) ? JsonConvert.SerializeObject(new Journey()) : identifiedModel.JourneyJson;
            return new Tuple<string, JourneyViewModel>("../JustToBeSafe/JustToBeSafe", identifiedModel);

        }

        private async Task<JustToBeSafeViewModel> BuildIdentifiedModel(JustToBeSafeViewModel model)
        {
            var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwayIdUrl(model.PathwayNo, model.UserInfo.Gender, model.UserInfo.Age)));

            if (pathway == null) return null;

            model.PathwayId = pathway.Id;
            model.PathwayTitle = pathway.Title;
            model.PathwayNo = pathway.PathwayNo;
            model.State = JourneyViewModelStateBuilder.BuildState(model.UserInfo.Gender,model.UserInfo.Age, _mappingEngine, model.State);
            model.StateJson = JourneyViewModelStateBuilder.BuildStateJson(model.State);
            return model;
        }
    }

    public interface IJustToBeSafeFirstViewModelBuilder
    {
        Task<Tuple<string, JourneyViewModel>> JustToBeSafeFirstBuilder(JustToBeSafeViewModel model);
    }
}
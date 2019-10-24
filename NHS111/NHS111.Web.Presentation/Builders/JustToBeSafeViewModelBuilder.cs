using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using RestSharp;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    using System.Net.Http;
    using System.Text;
    using System.Web;

    public class JustToBeSafeViewModelBuilder : BaseBuilder, IJustToBeSafeViewModelBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IRestClient _restClient;
        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;

        public JustToBeSafeViewModelBuilder(IJourneyViewModelBuilder journeyViewModelBuilder, IRestClient restClient, IConfiguration configuration, IMappingEngine mappingEngine)
        {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _restClient = restClient;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
        }

        public async Task<Tuple<string, JourneyViewModel>> JustToBeSafeNextBuilder(JustToBeSafeViewModel model)
        {
            model.State = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.StateJson);
            var questionsWithAnswers = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(model.QuestionsJson);
            var selectedQuestion = questionsWithAnswers.FirstOrDefault(q => q.Question.Id == model.SelectedQuestionId);

            var selectedAnswer = model.SelectedNoneApply()
                ? new Answer()
                : selectedQuestion.Answers.FirstOrDefault(a => a.Title.ToLower().StartsWith("yes")) ?? new Answer();

            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson).Add(
                new Journey
                {
                    Steps = questionsWithAnswers.
                        Select(q => new JourneyStep
                        {
                            QuestionId = q.Question.Id,
                            QuestionNo = q.Question.QuestionNo,
                            QuestionTitle = q.Question.Title,
                            Answer = q.Question.Id == model.SelectedQuestionId
                                ? selectedAnswer
                                : q.Answers.First(a => a.Title.ToLower().StartsWith("no")),
                            IsJustToBeSafe = true
                        }).ToList()
                });

            var url = _configuration.GetBusinessApiJustToBeSafePartTwoUrl(model.PathwayId,
                model.SelectedQuestionId ?? "",
                String.Join(",", questionsWithAnswers.Select(question => question.Question.Id)),
                selectedQuestion != null && selectedQuestion.Answers.Count > 3);
            var response = await _restClient.ExecuteTaskAsync<IEnumerable<QuestionWithAnswers>>(new JsonRestRequest(url, Method.GET));

            CheckResponse(response);

            var questions = response.Data.ToList();
            journey.Steps = journey.Steps.Where(step => !questions.Exists(question => question.Question.Id == step.QuestionId)).ToList();

            return await BuildNextAction(questions, journey, model, selectedAnswer, selectedQuestion, JsonConvert.SerializeObject(response.Data));
        }

        public async Task<Tuple<string, JourneyViewModel>> BuildNextAction(List<QuestionWithAnswers> questions, Journey journey, JustToBeSafeViewModel model, Answer selectedAnswer, QuestionWithAnswers selectedQuestion, string questionsJson)
        {
            if (!questions.Any())
            {
                journey.Steps = journey.Steps.Where(step => step.QuestionId != model.SelectedQuestionId).ToList();
                var questionViewModel = new QuestionViewModel
                {
                    PathwayId = model.PathwayId,
                    PathwayNo = model.PathwayNo,
                    PathwayTitle = model.PathwayTitle,
                    SymptomDiscriminatorCode = model.SymptomDiscriminatorCode,
                    UserInfo = model.UserInfo,
                    JourneyJson = JsonConvert.SerializeObject(journey),
                    SelectedAnswer = JsonConvert.SerializeObject(selectedAnswer),
                };

                _mappingEngine.Mapper.Map(selectedQuestion, questionViewModel);
                questionViewModel = _mappingEngine.Mapper.Map(selectedAnswer, questionViewModel);
                var nextNode = await GetNextNode(questionViewModel);
                return  new Tuple<string, JourneyViewModel>("TODO NOT USED?", await _journeyViewModelBuilder.Build(questionViewModel, nextNode));
            }

            if (questions.Count() == 1)
            {
                var journeyViewModel = new JourneyViewModel
                {
                    PathwayId = model.PathwayId,
                    PathwayNo = model.PathwayNo,
                    PathwayTitle = model.PathwayTitle,
                    UserInfo = model.UserInfo,
                    JourneyJson = JsonConvert.SerializeObject(journey),
                };

                _mappingEngine.Mapper.Map(questions.First(), journeyViewModel);
                journeyViewModel = _mappingEngine.Mapper.Map(selectedAnswer, journeyViewModel);

                return new Tuple<string, JourneyViewModel>("../Question/Question", journeyViewModel);
            }

            var viewModel = new JustToBeSafeViewModel
            {
                PathwayId = model.PathwayId,
                PathwayNo = model.PathwayNo,
                PathwayTitle = model.PathwayTitle,
                JourneyJson = JsonConvert.SerializeObject(journey),
                SymptomDiscriminatorCode = selectedAnswer.SymptomDiscriminator ?? model.SymptomDiscriminatorCode,
                Part = model.Part + 1,
                Questions = questions,
                QuestionsJson = questionsJson,
                UserInfo = model.UserInfo
            };

            return new Tuple<string, JourneyViewModel>("JustToBeSafe", viewModel);

        }

        private async Task<QuestionWithAnswers> GetNextNode(QuestionViewModel model) {
            var selectedAnswerState = Mapper.Map(model, new SelectedAnswerState());

            var businessApiNextNodeUrl = _configuration.GetBusinessApiNextNodeUrl(model.PathwayId, model.NodeType, model.Id);

            var request = new JsonRestRequest(businessApiNextNodeUrl, Method.POST);
            request.AddJsonBody(selectedAnswerState);
            var response = await _restClient.ExecuteTaskAsync<QuestionWithAnswers>(request);

            CheckResponse(response);

            return response.Data;
        }

    }

    public interface IJustToBeSafeViewModelBuilder
    {
        Task<Tuple<string, JourneyViewModel>> JustToBeSafeNextBuilder(JustToBeSafeViewModel model);
        Task<Tuple<string, JourneyViewModel>> BuildNextAction(List<QuestionWithAnswers> questions, Journey journey, JustToBeSafeViewModel model, Answer selectedAnswer, QuestionWithAnswers selectedQuestion, string questionsJson);
    }
}
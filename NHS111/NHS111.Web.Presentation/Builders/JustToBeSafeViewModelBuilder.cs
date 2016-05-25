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
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class JustToBeSafeViewModelBuilder : IJustToBeSafeViewModelBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IQuestionViewModelBuilder _questionViewModelBuilder;

        public JustToBeSafeViewModelBuilder(IQuestionViewModelBuilder questionViewModelBuilder, IRestfulHelper restfulHelper,
            IConfiguration configuration, IMappingEngine mappingEngine)
        {
            _questionViewModelBuilder = questionViewModelBuilder;
            _restfulHelper = restfulHelper;
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

            var questionsJson = await _restfulHelper.GetAsync(_configuration.GetBusinessApiJustToBeSafePartTwoUrl(model.PathwayId,
                model.SelectedQuestionId ?? "", String.Join(",", questionsWithAnswers.Select(question => question.Question.Id)), selectedQuestion != null && selectedQuestion.Answers.Count > 3));

            var questions = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(questionsJson);
            journey.Steps = journey.Steps.Where(step => !questions.Exists(question => question.Question.Id == step.QuestionId)).ToList();

            return await BuildNextAction(questions, journey, model, selectedAnswer, selectedQuestion, questionsJson);


        }

        public async Task<Tuple<string, JourneyViewModel>> BuildNextAction(List<QuestionWithAnswers> questions, Journey journey, JustToBeSafeViewModel model, Answer selectedAnswer, QuestionWithAnswers selectedQuestion, string questionsJson)
        {
            if (!questions.Any())
            {
                journey.Steps = journey.Steps.Where(step => step.QuestionId != model.SelectedQuestionId).ToList();
                var journeyViewModel = new JourneyViewModel
                {
                    PathwayId = model.PathwayId,
                    PathwayNo = model.PathwayNo,
                    PathwayTitle = model.PathwayTitle,
                    SymptomDiscriminator = model.SymptomDiscriminator,
                    UserInfo = model.UserInfo,
                    JourneyJson = JsonConvert.SerializeObject(journey),
                    SelectedAnswer = JsonConvert.SerializeObject(selectedAnswer),
                };

                _mappingEngine.Mapper.Map(selectedQuestion, journeyViewModel);
                journeyViewModel = _mappingEngine.Mapper.Map(selectedAnswer, journeyViewModel);
                
                return await _questionViewModelBuilder.BuildQuestion(journeyViewModel);
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
                SymptomDiscriminator = selectedAnswer.SymptomDiscriminator ?? model.SymptomDiscriminator,
                Part = model.Part + 1,
                Questions = questions,
                QuestionsJson = questionsJson,
                UserInfo = model.UserInfo
            };

            return new Tuple<string, JourneyViewModel>("JustToBeSafe", viewModel);

        }
    }

    public interface IJustToBeSafeViewModelBuilder
    {
        Task<Tuple<string, JourneyViewModel>> JustToBeSafeNextBuilder(JustToBeSafeViewModel model);
        Task<Tuple<string, JourneyViewModel>> BuildNextAction(List<QuestionWithAnswers> questions, Journey journey, JustToBeSafeViewModel model, Answer selectedAnswer, QuestionWithAnswers selectedQuestion, string questionsJson);
    }
}
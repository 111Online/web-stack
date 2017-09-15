using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Neo4jClient.Cypher;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IGraphRepository _graphRepository; 

        public QuestionRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<QuestionWithAnswers> GetQuestion(string id)
        {
            return await _graphRepository.Client.Cypher.
                Match(string.Format("(q {{ id: \"{0}\" }})", id)).
                OptionalMatch("q-[a:Answer]->()").
                Return(q => new QuestionWithAnswers { Question = Return.As<Question>("q"), Answers = Return.As<List<Answer>>(string.Format("collect(a)")), Labels = q.Labels() }).
                ResultsAsync.
                FirstOrDefault();
        }

        public async Task<IEnumerable<Answer>> GetAnswersForQuestion(string id)
        {
            var res =  await _graphRepository.Client.Cypher.
                 //Match(string.Format("(:Question {{ id: \"{0}\" }})-[a:Answer]->()", id)).
                Match(string.Format("({{ id: \"{0}\" }})-[a]->()", id)).
                Return(a => Return.As<Answer>("a")).
                ResultsAsync;
            return res;
        }

        public async Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer)
        {
            var query = _graphRepository.Client.Cypher.
                Match(string.Format("(:{0}{{ id: \"{1}\" }})-[a:Answer]->(next)", nodeLabel, id)).
                Where(string.Format("lower(a.title) = '{0}'", answer.Replace("'", "\\'").ToLower())).
                OptionalMatch("next-[nextAnswer]->()").
                OptionalMatch("next-[typeOf]->(g:OutcomeGroup)").
                Return(next => new QuestionWithAnswers
                {
                    Question = Return.As<Question>("next"),
                    Answers = Return.As<List<Answer>>("collect(nextAnswer)"),
                    Labels = next.Labels(),
                    Answered = Return.As<Answer>("a"),
                    Group = Return.As<OutcomeGroup>("g")
                });
            var res = await query.
                ResultsAsync.
                FirstOrDefault();
            return res;
        }

        public async Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId)
        {
            return await _graphRepository.Client.Cypher.
               Match(string.Format("(:Pathway {{ id: \"{0}\" }})-[:BeginsWith]->(q)", pathwayId)).
               OptionalMatch("q-[a:Answer]->()").
               Return(q => new QuestionWithAnswers
               {
                   Question = Return.As<Question>("q"), 
                   Answers = Return.As<List<Answer>>(string.Format("collect(a)")), Labels = q.Labels()
               }).
               ResultsAsync.
               FirstOrDefault();
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string justToBeSafePart)
        {
            return await GetJustToBeSafeQuestions(string.Format("{0}-{1}", pathwayId, justToBeSafePart));
        }

        private async Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string justToBeSafePart)
        {
            return await _graphRepository.Client.Cypher.
                Match(string.Format("(q:Question {{ jtbs: \"{0}\" }})-[a:Answer]->()", justToBeSafePart)).
                Return(q => new QuestionWithAnswers { Question = Return.As<Question>("q"), Answers = Return.As<List<Answer>>(string.Format("collect(a)")), Labels = q.Labels() }).
                ResultsAsync;
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string selectedQuestionId, bool multipleChoice, string answeredQuestionIds)
        {
            var getNextQuestionWithPath = new Func<Task<QuestionWithAnswers>>(async () =>
            {
                var queryMatchParts = new List<string>();
                var queryWhereParts = new List<string>();

                var questionIds = answeredQuestionIds.Split(',').ToList();
                var questionIdsArray = string.Format("[{0}]", string.Join(",", questionIds.Select(questionId => string.Format("\"{0}\"", questionId))));
                for (var i = 0; i < questionIds.Count; i++)
                {
                    queryMatchParts.Add(string.Format("(q{0}:Question)-[a{0}:Answer]->", i));
                    queryWhereParts.Add(string.Format("q{0}.id in {1} and a{0}.title =~ '(?i)No'", i, questionIdsArray));
                }

                return await _graphRepository.Client.Cypher.
                    Match(string.Join("", queryMatchParts) + "(next:Question)-[nextAnswer:Answer]->()").
                    Where(string.Join(" and ", queryWhereParts)).
                    Return(next => new QuestionWithAnswers { Question = Return.As<Question>("next"), Answers = Return.As<List<Answer>>(string.Format("collect(nextAnswer)")), Labels = next.Labels() }).
                    ResultsAsync.
                    FirstOrDefault();
            });

            var questionWasSelected = !string.IsNullOrEmpty(selectedQuestionId);

            if (questionWasSelected && multipleChoice)
            {
                return await GetQuestion(selectedQuestionId).InList();
            }

            var nextQuestion = await (questionWasSelected ? GetNextQuestion(selectedQuestionId, "Question", "Yes") : getNextQuestionWithPath());


            if (nextQuestion == null || nextQuestion.Labels.FirstOrDefault() == "Outcome")
            {
                return Enumerable.Empty<QuestionWithAnswers>();
            }

            return nextQuestion.Question.IsJustToBeSafe()
                ? await GetJustToBeSafeQuestions(nextQuestion.Question.Jtbs)
                : nextQuestion.InList();
        }
    }

    public interface IQuestionRepository
    {
        Task<QuestionWithAnswers> GetQuestion(string id);
        Task<IEnumerable<Answer>> GetAnswersForQuestion(string id);
        Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer);
        Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string justToBeSafePart);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string selectedQuestionId, bool multipleChoice, string answeredQuestionIds);
    }
}
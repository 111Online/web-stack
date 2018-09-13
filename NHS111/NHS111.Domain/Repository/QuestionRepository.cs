﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Neo4jClient.Cypher;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
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

        public async Task<IEnumerable<QuestionWithAnswers>> GetPathwaysJourney(List<JourneyStep> steps, string startingPathwayId)
        {
            var startingPathwayQuery = AddMatchesForStartingPathway(_graphRepository.Client.Cypher, steps.First(), startingPathwayId);
            ICypherFluentQuery query = AddMatchesForSteps(startingPathwayQuery, steps, true);
            query = query
                .With("rows.question as question, rows.answer as answer")
                .OrderBy("rows.step")
                .Where("answer is not null");

            var resultquery = query.ReturnDistinct(question => new QuestionWithAnswers()
                {
                    Answered = Return.As<Answer>("answer"),
                    Question = Return.As<Question>("question"),
                    Labels = question.Labels()
                }
            );
            var questionsWithAnswers = await resultquery.ResultsAsync;
            return questionsWithAnswers;
        }

        public ICypherFluentQuery AddMatchesForStartingPathway(ICypherFluentQuery query, JourneyStep firstQuestionStep, string startingPathwayId)
        {
            var modifiedQuery = query.Match(String.Format("(q:Pathway{{id:'{0}'}})-[:BeginsWith]-(n)", startingPathwayId))
                .OptionalMatch(String.Format("(q:Pathway{{id:'{0}'}})-[:BeginsWith]-(n)-[a:Answer]->()",
                    startingPathwayId))
                .Where("a.title ='default' or a.title = '\"present\"'")
                .With("collect({question:q, answer:{}, step:-1.2}) + collect({question:n, answer:a, step:-1.1}) as rows,n")
                .OptionalMatch(String.Format("p = (n)-[a:Answer*0..3]->(t)-[:Answer]->(:Question{{id:'{0}'}})",
                    firstQuestionStep.QuestionId)).Where("all(rel in a where rel.name in ['default','\"present\"']) and t:Set OR t:Read")

                .With("nodes(p)AS nds, rels(p) AS rls, rows")
                    .Unwind("case when nds is null then 0 else range(1, length(nds)) end", "i")
                    .With("rows + collect({question:nds[i], answer:rls[i], step:-1}) as allrows")
                .Unwind("allrows", "rows");

             
            return modifiedQuery;
        }

        public ICypherFluentQuery AddMatchesForSteps(ICypherFluentQuery query, List<JourneyStep> steps, bool containsExistingRows)
        {
            var modifiedQuery = query;
            for (int index = 0; index < steps.Count; ++index)
            {
                modifiedQuery = modifiedQuery.Match(String.Format("(q:Question{{id:'{0}'}})-[a:Answer{{order:{1}}}]->(n)", steps[index].QuestionId, steps[index].Answer.Order));
                modifiedQuery = index == 0 && !containsExistingRows
                    ?  modifiedQuery.With(String.Format("collect({{question:q, answer:a, step:{0}}}) as rows", index))
                    : (index != steps.Count - 1 ) ? modifiedQuery.With(String.Format("rows + collect({{question:q, answer:a, step:{0}}}) as rows" ,index)) 
                        : modifiedQuery.With(String.Format("rows + collect({{question:q, answer:a, step:{0}}}) + collect({{question:n, answer:{{}}, step:{0}.1}}) as rows", index));
                if (index != steps.Count - 1)
                {
                    modifiedQuery = modifiedQuery.OptionalMatch(String.Format(
                        "p = (:Question{{id:'{0}'}})-[:Answer*0..3]->(t)-[:Answer]->(:Question{{id:'{1}'}})",
                        steps[index].QuestionId,
                        steps[index + 1].QuestionId));

                    modifiedQuery = modifiedQuery.Where("t:Set OR t:Read");
                    modifiedQuery = modifiedQuery.With("nodes(p)AS nds, rels(p) AS rls, rows")
                    .Unwind("case when nds is null then 0 else range(1, length(nds) - 2) end", "i")
                    .With(String.Format("rows + collect({{question:nds[i], answer:rls[i], step:{0}}}) as allrows",
                                index + 0.1));
                }

                modifiedQuery = (index != steps.Count - 1)
                    ?  modifiedQuery.Unwind("allrows", "rows")
                    : modifiedQuery.With("rows as allrows").Unwind("allrows", "rows");


            }
            return modifiedQuery;
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
        Task<IEnumerable<QuestionWithAnswers>>GetPathwaysJourney(List<JourneyStep> steps, string startingPathwayId);
        Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer);
        Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string justToBeSafePart);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string selectedQuestionId, bool multipleChoice, string answeredQuestionIds);
    }
}
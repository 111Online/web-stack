using System;
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


        public async Task<IEnumerable<QuestionWithRelatedAnswers>> GetPathwaysJourney(List<JourneyStep> steps, string startingPathwayId, string dispositionCode)
        {
            var startingPathwayQuery = AddMatchesForStartingPathway(_graphRepository.Client.Cypher, steps.First(), startingPathwayId);
            ICypherFluentQuery query = AddMatchesForSteps(startingPathwayQuery, steps, true, dispositionCode);
            query = query
                .With("rows.question as question, rows.answer as answer, rows.leadingAnswer as leadingAnswer, rows.answers as answers")
                .OrderBy("rows.step")
                .Where("answer is not null and  labels(question) is not null and NOT \"PathwaySelectionJump\" IN labels(question)");

            var resultquery = query.ReturnDistinct(question => new QuestionWithRelatedAnswers()
                {
                    Answered = Return.As<Answer>("answer"),
                    LeadingAnswer = Return.As<Answer>("leadingAnswer"),
                    Question = Return.As<Question>("question"),
                    Answers = Return.As<List<Answer>>("answers"),
                Labels = question.Labels()
                }
            );
            var questionWithAnswerses = await resultquery.ResultsAsync;
            return questionWithAnswerses;

        }

        public ICypherFluentQuery AddMatchesForSteps(ICypherFluentQuery query, List<JourneyStep> steps)
        {
            var modifiedQuery = query;
            for (int index = 0; index < steps.Count; ++index)
            {
                 modifiedQuery = modifiedQuery.Match(String.Format("(q:Question{{id:'{0}'}})-[a:Answer{{order:{1}}}]->(n)", steps[index].QuestionId,steps[index].Answer.Order));
                modifiedQuery = index == steps.Count - 1 ? 
                    modifiedQuery.OptionalMatch("(n)-[b:Answer]->(r)") : 
                    modifiedQuery.OptionalMatch(String.Format("(n)-[b:Answer]->(r:Question{{id:'{0}'}})", steps[index + 1].QuestionId));
                modifiedQuery = (index <= 0 ? 
                    modifiedQuery.With(String.Format("collect({{question:q, answer:a, step:{0}}})as rows, n,b", index)) : 
                    modifiedQuery.With(String.Format("rows + collect({{question:q, answer:a, step:{0}}})as rows, n,b", index))).
                    With(String.Format("rows + collect({{question:n, answer:b, step:{0}}}) as allrows", index + 0.1)).Unwind("allrows", "rows");
            }
            return modifiedQuery;
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

        public ICypherFluentQuery AddMatchesForSteps(ICypherFluentQuery query, List<JourneyStep> steps, bool containsExistingRows, string dispositionCode)
        {
            var modifiedQuery = query;
            for (int index = 0; index < steps.Count; ++index)
            {
                if (!IsLastStep(steps, index))
                    modifiedQuery = modifiedQuery.Match(String.Format("(q:Question{{id:'{0}'}})-[a:Answer]->(n)",
                        steps[index].QuestionId));

                modifiedQuery = index == 0 && !containsExistingRows
                    ? modifiedQuery
                        .With(String.Format(
                            "q, COLLECT(distinct a) as answers, filter(x IN COLLECT(distinct a) WHERE x.order= {0})[0]  as answered",
                            steps[index].Answer.Order))
                        .With(String.Format(
                            "collect({{question:q, answer:answered, answers:answers, step:{0}}}) as rows", index))

                    : (!IsLastStep(steps, index))
                        ? modifiedQuery
                            .With(String.Format(
                                "rows,q, COLLECT(distinct a) as answers, filter(x IN COLLECT(distinct a) WHERE x.order= {0})[0]  as answered",
                                steps[index].Answer.Order))
                            .With(String.Format(
                                "rows + collect({{question:q, answer:answered, answers:answers, step:{0}}}) as rows",
                                index))
                        : modifiedQuery
                            .OptionalMatch(String.Format("(q:Question{{id:'{0}'}})-[a:Answer]->()", steps[index].QuestionId))
                            .With(String.Format(
                                "rows,q, COLLECT(distinct a) as answers, filter(x IN COLLECT(distinct a) WHERE x.order= {0})[0]  as answered",
                                steps[index].Answer.Order))
                            .With(String.Format(
                                "rows + collect({{question:q, answer:answered, answers:answers, step:{0}}}) as rows", index))
                            .OptionalMatch(String.Format("(q:Question{{id:'{0}'}})-[a:Answer{{order:{1}}}]->(n{{id:'{2}'}})",
                                steps[index].QuestionId, steps[index].Answer.Order, dispositionCode))
                            .With(String.Format("rows + collect({{question:n, answer:{{}}, step:{0}.1}}) as allrows", index))
                            .Unwind("allrows", "rows")

                            .OptionalMatch(String.Format(
                                "p = (:Question{{id:'{0}'}})-[a:Answer{{order:{1}}}]->(f)-[:Answer*0..3]->(t)-[:Answer]->(n{{id:'{2}'}})", steps[index].QuestionId, steps[index].Answer.Order, dispositionCode))
                            .Where("(t:Set OR t:Read) and (f:Set OR f:Read)")
                            .With("nodes(p)AS nds, rels(p) AS rls, rows, n")

                            //.Match("(n1)-[a:Answer]->()")
                            //.Where("n1 in nds AND (n1:Set OR n1:Read)")
                            //.With("nds, rls, rows, n, n1, {leadingnode:n1, nodeanswers:COLLECT(DISTINCT a)} as node")


                            .Unwind("case when nds is null then 0 else range(1, length(nds) - 2) end", "i")

                            .With(String.Format(
                                "rows + collect({{question:nds[i], answer:rls[i], leadingAnswer:rls[i-1], step:{0}.2}}) + collect({{question:n, answer:{{}}, step:{0}.3}}) as newrows",
                                index));
                            //.With(String.Format(
                            //    "rows + collect({{question:nds[i], answer:rls[i], answers:CASE WHEN nds[i] = node.leadingnode THEN node.nodeanswers ELSE null END, leadingAnswer:rls[i-1], step:{0}.2}}) + collect({{question:n, answer:{{}}, step:{0}.3}}) as newrows",
                            //    index));

                if (!IsLastStep(steps, index))
                {
                    modifiedQuery = modifiedQuery.OptionalMatch(String.Format(
                        "p = (:Question{{id:'{0}'}})-[a:Answer{{order:{1}}}]-(f)-[:Answer*0..3]->(t)-[:Answer]->(:Question{{id:'{2}'}})",
                        steps[index].QuestionId,
                        steps[index].Answer.Order,
                        steps[index + 1].QuestionId));

                    modifiedQuery = modifiedQuery.Where("(t:Set OR t:Read) and (f:Set OR f:Read)");
                    modifiedQuery = modifiedQuery.With("nodes(p)AS nds, rels(p) AS rls, rows")

                        //.Match("(n1)-[a:Answer]->()")
                        //.Where("n1 in nds AND (n1:Set OR n1:Read)")
                        //.With("nds, rls, rows, n1, {leadingnode:n1, nodeanswers:COLLECT(DISTINCT a)} as node")

                        .Unwind("case when nds is null then 0 else range(1, length(nds) - 2) end", "i")

                        .With(String.Format(
                            "rows + collect({{question:nds[i], answer:rls[i], leadingAnswer:rls[i-1], step:{0}}}) as newrows",
                            index + 0.1));
                        //.With(String.Format(
                        //    "rows + collect({{question:nds[i], answer:rls[i], answers:CASE WHEN nds[i] = node.leadingnode THEN node.nodeanswers ELSE null END, leadingAnswer:rls[i-1], step:{0}}}) as newrows",
                        //    index + 0.1));
                }

                modifiedQuery = modifiedQuery.Unwind("newrows", "rows");



            }
            return modifiedQuery;
        }

        private bool IsLastStep(List<JourneyStep> steps, int index)
        {
            return index == steps.Count - 1;
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

        Task<IEnumerable<QuestionWithRelatedAnswers>>
            GetPathwaysJourney(List<JourneyStep> steps, string startingPathwayId, string dispositionCode);
        Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer);
        Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string justToBeSafePart);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string selectedQuestionId, bool multipleChoice, string answeredQuestionIds);
    }
}
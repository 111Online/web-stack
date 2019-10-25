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
                Match(string.Format("({{ id: \"{0}\" }})-[a:Answer]->()", id)).
                Return(a => Return.As<Answer>("a")).
                ResultsAsync;
            return res;
        }

        public async Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer)
        {
            var query = _graphRepository.Client.Cypher.
                Match(string.Format("(:{0}{{ id: \"{1}\" }})-[a:Answer]->(next)", nodeLabel, id)).
                Where(string.Format("lower(a.title) = '{0}'", answer.Replace("'", "\\'").ToLower())).
                OptionalMatch("next-[nextAnswer:Answer]->()").
                OptionalMatch("next-[:typeOf]->(g:OutcomeGroup)").
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


        public async Task<IEnumerable<QuestionWithAnswers>> GetPathwaysJourney(List<JourneyStep> steps, string startingPathwayId, string dispositionCode, string gender, int age)
        {
            var startingPathwayQuery = AddMatchesForStartingPathway(_graphRepository.Client.Cypher, steps.First(), startingPathwayId);
            ICypherFluentQuery query = AddMatchesForSteps(startingPathwayQuery, steps, true, dispositionCode,  gender,  age);
            query = query
                .With("rows.question as question, rows.answer as answer,rows.pathway as pathway, rows.answers as answers")
                .OrderBy("rows.step")
                .Where("answer is not null and  labels(question) is not null");

            var resultquery = query.ReturnDistinct(question => new QuestionWithAnswers()
                {
                    Answered = Return.As<Answer>("answer"),
                    Question = Return.As<Question>("question"),
                    Answers = Return.As<List<Answer>>("answers"),
                    AssociatedPathway = Return.As<Pathway>("pathway"),
                Labels = question.Labels()
                }
            );
            var questionWithAnswerses = await resultquery.ResultsAsync;
            return questionWithAnswerses;

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

        public ICypherFluentQuery AddMatchesForSteps(ICypherFluentQuery query, List<JourneyStep> steps, bool containsExistingRows, string dispositionCode, string gender, int age)
        {
            var modifiedQuery = query;
            for (int index = 0; index < steps.Count; ++index)
            {
                if (!IsLastStep(steps, index))
                    modifiedQuery = modifiedQuery.Match(String.Format("(q:{0}{{id:'{1}'}})-[a:Answer]->(n)",
                        steps[index].NodeLabel, steps[index].QuestionId));

                modifiedQuery = index == 0 && !containsExistingRows
                    ? modifiedQuery
                        .OptionalMatch("q-[:ofPathway]->(pw:Pathway)").Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("pw", gender), FilterStatements.AgeIsAboveMinimum("pw", age), FilterStatements.AgeIsBelowMaximum("pw", age) }))
                        .With(String.Format(
                            "q,pw, COLLECT(distinct a) as answers, filter(x IN COLLECT(distinct a) WHERE x.order= {0})[0]  as answered",
                            steps[index].Answer.Order))
                        .With(String.Format(
                            "collect({{question:q, answer:answered, answers:answers,pathway:pw, step:{0}}}) as rows", index))

                    : (!IsLastStep(steps, index))
                        ? modifiedQuery
                            .OptionalMatch("q-[:ofPathway]->(pw:Pathway)").Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("pw", gender), FilterStatements.AgeIsAboveMinimum("pw", age), FilterStatements.AgeIsBelowMaximum("pw", age) }))
                            .With(String.Format(
                                "rows,q,pw, COLLECT(distinct a) as answers, filter(x IN COLLECT(distinct a) WHERE x.order= {0})[0]  as answered",
                                steps[index].Answer.Order))
                            .With(String.Format(
                                "rows + collect({{question:q, answer:answered, answers:answers,pathway:pw, step:{0}}}) as rows",
                                index))
                        : modifiedQuery
                            .OptionalMatch(String.Format("(q:{0}{{id:'{1}'}})-[a:Answer]->()", steps[index].NodeLabel, steps[index].QuestionId))
                            .OptionalMatch("q-[:ofPathway]->(pw:Pathway)").Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("pw", gender), FilterStatements.AgeIsAboveMinimum("pw", age), FilterStatements.AgeIsBelowMaximum("pw", age) }))
                            .With(String.Format(
                                "rows,q,pw, COLLECT(distinct a) as answers, filter(x IN COLLECT(distinct a) WHERE x.order= {0})[0]  as answered",
                                steps[index].Answer.Order))
                            .With(String.Format(
                                "rows + collect({{question:q, answer:answered, answers:answers,pathway:pw, step:{0}}}) as rows", index))
                            .OptionalMatch(String.Format("(q:{0}{{id:'{1}'}})-[a:Answer{{order:{2}}}]->(n{{id:'{3}'}})",
                                steps[index].NodeLabel, steps[index].QuestionId, steps[index].Answer.Order, dispositionCode))
                            .OptionalMatch("n-[i:Instruction]->(:OutcomeEnd)")
                            .With("rows,n, collect(i) as instructions")
                            .With(String.Format("rows + collect({{question:n, answer:{{}}, answers:instructions, step:{0}.1}}) as allrows", index))
                            .Unwind("allrows", "rows")

                            .OptionalMatch(String.Format(
                                "p = (:{0}{{id:'{1}'}})-[a:Answer{{order:{2}}}]->(f)-[:Answer*0..3]->(t)-[:Answer]->(n{{id:'{3}'}})", steps[index].NodeLabel, steps[index].QuestionId, steps[index].Answer.Order, dispositionCode))
                            .Where("(t:Set OR t:Read) and (f:Set OR f:Read)")
                            .OptionalMatch("n-[i:Instruction]->(:OutcomeEnd)")
                            .With("nodes(p)AS nds, rels(p) AS rls, rows, n, collect(distinct i) as instructions")

                            .Unwind("coalesce(nds, [null])", "n1")
                            .OptionalMatch("(n1)-[a:Answer]->()")
                            .OptionalMatch("(n1)-[:ofPathway]->(pw:Pathway)").Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("pw", gender), FilterStatements.AgeIsAboveMinimum("pw", age), FilterStatements.AgeIsBelowMaximum("pw", age) }))
                            .With("nds,pw, rls, rows, n, {leadingnode:n1, nodeanswers:COLLECT(DISTINCT a)} as node, instructions")

                            .Unwind("case when nds is null then 0 else range(1, length(nds) - 2) end", "x")
                            
                            .With(String.Format(
                                "rows + collect({{question:nds[x], answer:CASE WHEN nds[x] = node.leadingnode THEN CASE WHEN type(rls[x]) = 'Answer' THEN rls[x] ELSE null END ELSE null END, answers:node.nodeanswers,pathway:pw, step:{0}.2}}) + collect({{question:n, answer:{{}}, answers:instructions, step:{0}.3}}) as newrows",
                                index));

                if (!IsLastStep(steps, index))
                {
                    modifiedQuery = modifiedQuery.OptionalMatch(String.Format(
                        "p = (:{0}{{id:'{1}'}})-[a:Answer{{order:{2}}}]->(f)-[:Answer*0..3]->(t)-[:Answer]->(:{3}{{id:'{4}'}})",
                        steps[index].NodeLabel,
                        steps[index].QuestionId,
                        steps[index].Answer.Order,
                        steps[index + 1].NodeLabel,
                        steps[index + 1].QuestionId)).Where("(t:Set OR t:Read) and (f:Set OR f:Read)");



                    modifiedQuery = modifiedQuery.With("nodes(p)AS nds, rels(p) AS rls, rows")

                        .Unwind("coalesce(nds, [null])", "n1")
                        .OptionalMatch("(n1)-[a:Answer]->()")
                        .OptionalMatch("(n1)-[:ofPathway]->(pw:Pathway)").Where(string.Join(" and ", new List<string> { FilterStatements.GenderIs("pw", gender), FilterStatements.AgeIsAboveMinimum("pw", age), FilterStatements.AgeIsBelowMaximum("pw", age) }))
                        .With("nds,pw, rls, rows, n1, {leadingnode:n1, nodeanswers:COLLECT(DISTINCT a)} as node")

                        .Unwind("case when nds is null then 0 else range(1, length(nds) - 2) end", "i")

                        .With(String.Format(
                            "rows + collect({{question:nds[i], answer:CASE WHEN nds[i] = node.leadingnode THEN rls[i] ELSE null END, answers:node.nodeanswers,pathway:pw, step:{0}+toFloat(i)/10}}) as newrows",
                            index + 0.1));
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

        Task<IEnumerable<QuestionWithAnswers>>
            GetPathwaysJourney(List<JourneyStep> steps, string startingPathwayId, string dispositionCode, string gender,
                int age);
        Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer);
        Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string justToBeSafePart);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestions(string pathwayId, string selectedQuestionId, bool multipleChoice, string answeredQuestionIds);
    }
}
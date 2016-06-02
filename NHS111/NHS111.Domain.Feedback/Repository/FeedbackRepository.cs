using System.Collections.Generic;
using System.Threading.Tasks;
using NHS111.Utils.Configuration;
using NHS111.Utils.Converters;

namespace NHS111.Domain.Feedback.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private IConnectionManager _sqliteConnectionManager;
        private IDataConverter<Models.Models.Domain.Feedback> _feedbackConverter;

        public FeedbackRepository(IConnectionManager sqlConnectionManager, IDataConverter<Models.Models.Domain.Feedback> feedbackConverter)
        {
            _sqliteConnectionManager = sqlConnectionManager;
            _feedbackConverter = feedbackConverter;
        }

        public async Task<int> Add(Models.Models.Domain.Feedback feedback)
        {
            var statementParameters = _feedbackConverter.Convert(feedback);
            var insertQuery = statementParameters.GenerateInsertStatement("feedback");
            return await _sqliteConnectionManager.ExecteNonQueryAsync(insertQuery, statementParameters);
        }

        public async Task<IEnumerable<Models.Models.Domain.Feedback>> List()
        {
            string selectStatement = string.Format("{0}{1}{2}{3}", 
                "SELECT ",
                string.Join(",", _feedbackConverter.Fields()),
                " FROM feedback",
                " ORDER BY feedbackDate DESC");

            Task<List<Models.Models.Domain.Feedback>> task = new Task<List<Models.Models.Domain.Feedback>>
                (
                () =>
                {
                    var feedbackList = new List<Models.Models.Domain.Feedback>();
                    using ( IManagedDataReader reader = _sqliteConnectionManager.GetReader(selectStatement, new StatementParameters()))
                    {
                        while (reader.Read())
                        {
                            feedbackList.Add(_feedbackConverter.Convert(reader));
                        }
                    }

                    return feedbackList;
                });
            task.Start();

            return await task;
        }
    }
}

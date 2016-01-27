using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Configuration
{
    public class SqliteConnectionManager
    {

        private SQLiteConnection _diskDbConnection;
        private string _dbFileLocation;
        private string _connectionString;

        public SqliteConnectionManager(string dbFileLocation)
        {
            _dbFileLocation = dbFileLocation;
            _connectionString = "data source=" + _dbFileLocation + "; Version=3; Pooling=True; Max Pool Size=100;";
            _diskDbConnection = new SQLiteConnection(_connectionString);
        }

        public IManagedDataReader GetReader(string statement, StatementParamaters statementParamaters)
        {
            SQLiteCommand command = new SQLiteCommand(statement, _diskDbConnection);

            foreach (var parameter in statementParamaters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return new ManagedDataReader(command);
        }

        public void ExecteNonQuery(string statement, StatementParamaters statementParamaters)
        {
            SQLiteCommand insertCommand = new SQLiteCommand(statement, _diskDbConnection);
            foreach (var parameter in statementParamaters)
            {
                insertCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);

                insertCommand.ExecuteNonQuery();
            }

        }
    }
    public class StatementParamaters : Dictionary<string, object>
    { }
}

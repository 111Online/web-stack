using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Configuration
{
    public class SqliteConnectionManager : IConnectionManager
    {

        private SQLiteConnection _diskDbConnection;
        private string _dbFileLocation;
        private string _connectionString;

        private ISqliteConfiguration _sqliteConfiguration;

        public SqliteConnectionManager(ISqliteConfiguration configuration)
        {
            _connectionString = configuration.GetSqliteConnectionString();
            _diskDbConnection = new SQLiteConnection(_connectionString);
        }
        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_diskDbConnection.ConnectionString);
        }

        public IManagedDataReader GetReader(string statement, StatementParamaters statementParamaters)
        {
            SQLiteCommand command = new SQLiteCommand(statement, GetConnection());

            foreach (var parameter in statementParamaters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return new ManagedDataReader(command);
        }

        public int ExecteNonQuery(string statement, StatementParamaters statementParamaters)
        {
            using (var conn = GetConnection())
            {
                SQLiteCommand insertCommand = new SQLiteCommand(statement, conn);
                foreach (var parameter in statementParamaters)
                {
                    insertCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                conn.Open();
                var returnVal= insertCommand.ExecuteNonQuery();
                conn.Close();
                return returnVal;
            }
        }

        public async Task<int> ExecteNonQueryAsync(string statement, StatementParamaters statementParamaters)
        {
            using (var conn = GetConnection())
            {
                SQLiteCommand insertCommand = new SQLiteCommand(statement, conn);
                foreach (var parameter in statementParamaters)
                {
                    insertCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                conn.Open();
                var returnVal = await insertCommand.ExecuteNonQueryAsync();
                conn.Close();
                return returnVal;
            }
        }
    }


}

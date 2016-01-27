using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Configuration
{
    public class ManagedDataReader : IManagedDataReader
    {
        private IDbConnection _connection;
        private IDataReader _dataReader;
        private IDbCommand _command;
        public ManagedDataReader(IDbCommand command)
        {
            _command = command;
            _connection = command.Connection;
            if (_connection.State != ConnectionState.Open) _connection.Open();
            _dataReader = command.ExecuteReader();

        }

        public IDataReader DataReader
        {
            get { return _dataReader; }
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public bool Read()
        {
            return _dataReader.Read();
        }

        public void Dispose()
        {
            _command.Dispose();
            _dataReader.Close();
            _connection.Close();

            _dataReader.Dispose();
            _connection.Dispose();
        }
    }
}

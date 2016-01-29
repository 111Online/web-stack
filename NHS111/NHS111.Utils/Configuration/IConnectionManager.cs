using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Configuration
{
    public interface IConnectionManager
    {
        IManagedDataReader GetReader(string statement, StatementParamaters paramsCollection);
        int ExecteNonQuery(string statement, StatementParamaters statementParamaters);
        Task<int> ExecteNonQueryAsync(string statement, StatementParamaters statementParamaters);
    }
}

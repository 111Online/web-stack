using System.Collections.Generic;
using System.Text;

namespace NHS111.Utils.Configuration
{
    public class StatementParameters : Dictionary<string, object>
    {
        public string GenerateInsertStatement(string tableName)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO " + tableName + " (");

            AddFieldList(builder);
            builder.Append(") ");

            builder.Append("VALUES(");

            AddParameterList(builder);
            builder.Append(")");
            return builder.ToString();
        }

        private void AddFieldList(StringBuilder builder)
        {
            AddParameterList(builder, false);
        }

        private void AddParameterList(StringBuilder builder, bool asParamter = true)
        {
            int i = 0;
            foreach (var key in this.Keys)
            {
                i++;
                if (asParamter && !key.StartsWith("@")) builder.Append("@");
                builder.Append(key);
                if (i < this.Keys.Count) builder.Append(",");
            }
        }
    }
}
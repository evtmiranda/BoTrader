using System.Collections.Generic;
using System.Data.SqlClient;

namespace BotTrader.Service
{
    internal class Serializacao
    {
        public IEnumerable<Dictionary<string, object>> Serializar(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializarRow(cols, reader));

            return results;
        }

        private Dictionary<string, object> SerializarRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
    }
}

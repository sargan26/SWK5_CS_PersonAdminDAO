using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate T RowMapper<T>(IDataRecord row); //delegate hier wahnsinnig mächtig, anschauen!

namespace Dal.Common
{
    public class AdoTemplate(IConnectionFactory connectionFactory)
    {
        //prepared statement
        private void AddParameters(DbCommand command, QueryParameter[] parameters)
        {
            foreach (var p in parameters)
            {
                DbParameter dbParam = command.CreateParameter();
                dbParam.ParameterName = p.Name;
                dbParam.Value = p.Value;

                command.Parameters.Add(dbParam);
            }
        }


        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parametes)
        {
            await using DbConnection connection = await connectionFactory.CreateConnectionAsync();

            // hier durch await wird asynchrone dispose methode aufgerufen
            await using DbCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            AddParameters(command, parametes);

            await using DbDataReader reader = await command.ExecuteReaderAsync();

            IList<T> persons = [];
            while (await reader.ReadAsync())
            {
                persons.Add(rowMapper(reader));
            }
            return persons;
        }

        public async Task<int> ExecuteAsync(string sql, params QueryParameter[] parametes)
        {
            await using DbConnection connection = await connectionFactory.CreateConnectionAsync();

            await using DbCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            AddParameters(command, parametes);

            return await command.ExecuteNonQueryAsync();
        }

        //public T? QuerySingle<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters)
        //{
        //    return QueryAsync(sql, rowMapper, parameters).SingleOrDefault();
        //}

        public async Task<T?> QuerySingleAsync<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters)
        {
            var results = await QueryAsync(sql, rowMapper, parameters);
            return results.SingleOrDefault();
        }

    }
}

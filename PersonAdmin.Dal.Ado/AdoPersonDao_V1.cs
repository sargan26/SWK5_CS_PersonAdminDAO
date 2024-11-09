using Dal.Common;
using Microsoft.Data.SqlClient;
using PersonAdmin.Dal.Interface;
using PersonAdmin.Domain;

namespace PersonAdmin.Dal.Ado
{
    public class AdoPersonDao_V1 : IPersonDao
    {
        public Task<IEnumerable<Person>> IPersonDao.FindAllAsync()
        {
            var (connectionString, _) = ConfigurationUtil.GetConnectionParameters("PersonDbConnection", "ProviderName");

            // using nicht vergessen bei Connection
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("select * from person", connection);

            using SqlDataReader reader = command.ExecuteReader();

            IList <Person> persons = [];
            while(reader.Read())
            {
                persons.Add(new Person(
                    id: (int)reader["Id"],
                    firstName: (string)reader["first_name"],
                    lastName: (string)reader["last_name"],
                    dateOfBirth: (DateTime)reader["date_of_birth"]));
            }
            return persons;
        }

        Task<Person?> IPersonDao.FindById(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPersonDao.UpdateAsync(Person person)
        {
            throw new NotImplementedException();
        }
    }
}

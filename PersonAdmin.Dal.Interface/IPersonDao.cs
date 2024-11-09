using PersonAdmin.Domain;
using System.Data;

namespace PersonAdmin.Dal.Interface;

    public interface IPersonDao
    {
        Task<IEnumerable<Person>> FindAllAsync();
        Task<Person?> FindById(int id);
        Task<bool> UpdateAsync(Person person);
    }


using PersonAdmin;
using PersonAdmin.Domain;
using PersonAdmin.Dal.Interface;

namespace PersonAdmin.Dal.Simple;

public class SimplePersonDao : IPersonDao
{
   private static IList<Person> personList = new List<Person>
   {
     new (id: 1, firstName: "John", lastName: "Doe",        dateOfBirth: DateTime.Now.AddYears(-10)),
     new (id: 2, firstName: "Jane", lastName: "Doe",        dateOfBirth: DateTime.Now.AddYears(-20)),
     new (id: 3, firstName: "Max",  lastName: "Mustermann", dateOfBirth: DateTime.Now.AddYears(-30))
   };

    public Task<IEnumerable<Person>> FindAllAsync()
    {
        return Task.FromResult<IEnumerable<Person>>(personList);
    }

    public Task<Person?> FindById(int id)
    {
        return Task.FromResult(personList.SingleOrDefault(p => p.Id == id));
    }

    public async Task<bool> UpdateAsync(Person person)
    {
        var p = await FindById(person.Id);
        if (p is null) return false;
        p.FirstName = person.FirstName;
        p.LastName = person.LastName;
        p.DateOfBirth = person.DateOfBirth;

        return true;
    }
}


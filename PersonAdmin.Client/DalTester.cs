using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using PersonAdmin.Dal.Interface;
using PersonAdmin.Domain;

namespace PersonAdmin.Client
{
    internal class DalTester(IPersonDao personDao)
    {
        private readonly IPersonDao personDao = personDao;
        public void TestFindAll()
        {
            personDao.FindAllAsync()
                .ToList()
                .ForEach(p => Console.WriteLine($"{p.Id,5} | {p.FirstName,-10} | {p.LastName,-15} | {p.DateOfBirth,10:yyyy-MM-dd}"));
        }

        public void TestFindById()
        {
            Person? person1 = await personDao.FindById(1);
            Console.WriteLine($"person1 -> {person1?.ToString() ?? "<null>"}");
            Person? person2 = await personDao.FindById(9999);
            Console.WriteLine($"person1 -> {person2?.ToString() ?? "<null>"}");
        }

        public void TestUpdate()
        {
            Person? person = await personDao.FindById(1);
            if (person is null) return;
            Console.WriteLine($"before update: person -> {person}");

            person.DateOfBirth = person.DateOfBirth.AddYears(-1);
            personDao.UpdateAsync(person);
            person = await personDao.FindById(1);
            Console.WriteLine($"after update: person -> {person?.ToString() ?? "<null>"}");
        }

        public void TestTransactioins()
        {
            var person1 = personDao.FindById(1);
            var person2 = personDao.FindById(2);
            if (person1 is null || person2 is null) return;

            Console.WriteLine($"before tx: person1 -> {person1.ToString() ?? "<null>"}");
            Console.WriteLine($"before tx: person2 -> {person2.ToString() ?? "<null>"}");


            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    person1.DateOfBirth = person1.DateOfBirth.AddDays(1);
                    person2.DateOfBirth = person2.DateOfBirth.AddDays(1);

                    await personDao.UpdateAsync(person1);
                    //throw new Exception("Interrupted transaction");
                    await personDao.UpdateAsync(person2);

                    scope.Complete();
                }
            }
            catch { }

            person1 = await personDao.FindById(1);
            person2 = await personDao.FindById(2);

            Console.WriteLine($"after tx: person1 -> {person1?.ToString() ?? "<null>"}");
            Console.WriteLine($"after tx: person2 -> {person2?.ToString() ?? "<null>"}");

        }
    }
}

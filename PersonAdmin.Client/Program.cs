using Dal.Common;
using Microsoft.Extensions.Configuration;
using PersonAdmin.Client;
using PersonAdmin.Dal.Ado;
using PersonAdmin.Dal.Simple;

static void PrintTitle(string text = "", int length = 60, char ch = '-')
{
  int preLen = (length - (text.Length + 2)) / 2;
  int postLen = length - (preLen + text.Length + 2);
  Console.WriteLine($"{new String(ch, preLen)} {text} {new String(ch, postLen)}");
}

PrintTitle("SimplePersonDao.FindAll");
var tester1 = new DalTester(new SimplePersonDao()); // dependency injection
tester1.TestFindAll();

PrintTitle("SimplePersonDao.Update");
tester1.TestUpdate();

IConfiguration configuration = ConfigurationUtil.GetConfiguration();
IConnectionFactory connectionFactory =
    DefaultConnectionFactory.FromConfiguration(configuration, "PersonDbConnection", "ProviderName");

PrintTitle("AdoPersonDao.FindAll");
var tester2 = new DalTester(new AdoPersonDao(connectionFactory)); 
tester2.TestFindAll();


PrintTitle("AdoPersonDao.FindById");
tester2.TestFindById();

PrintTitle("AdoPersonDao.Update");
tester2.TestUpdate();

PrintTitle("AdoPersonDao.TestTransactions");
tester2.TestTransactioins();
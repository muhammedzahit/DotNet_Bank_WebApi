using bank_webapi.DbOperations;
using bank_webapi.Entities;

namespace bank_webapi_UnitTests.TestSetup;

public static class Users
{
    public static void AddUsers(this IBankDbContext context)
    {
        context.Users.Add(new User()
        {
            IsBanker = true,
            Name = "admin",
            Password = "admin",
            Iban = 0
        });
        context.Users.Add(new User()
        {
            IsBanker = false,
            Name = "client",
            Password = "client",
            Iban = 100,
            Capital = 10000,
            Investment = 10000
        });
    } 
}
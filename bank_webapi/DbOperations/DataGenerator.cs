using bank_webapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace bank_webapi.DbOperations;

public static class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = new BankDbContext(serviceProvider.GetRequiredService<DbContextOptions<BankDbContext>>());
        if (!context.Users.Any())
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
            context.SaveChanges();
        }
    } 
}
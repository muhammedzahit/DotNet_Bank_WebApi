using bank_webapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace bank_webapi.DbOperations;

public interface IBankDbContext
{
    DbSet<User> Users { get; set; }
    public int SaveChanges();
}
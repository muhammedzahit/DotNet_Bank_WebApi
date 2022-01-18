using bank_webapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace bank_webapi.DbOperations;

public class BankDbContext : DbContext, IBankDbContext
{
    
    public DbSet<User> Users { get; set; }
    
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
        
    }
    
    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
    
    
}
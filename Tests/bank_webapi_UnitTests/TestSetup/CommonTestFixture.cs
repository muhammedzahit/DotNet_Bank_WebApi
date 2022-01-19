using System.Collections.Generic;
using AutoMapper;
using bank_webapi.Commons;
using bank_webapi.DbOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace bank_webapi_UnitTests.TestSetup;

public class CommonTestFixture
{
    public BankDbContext Context { get; set; }
    public IMapper Mapper { get; set; }
    
    public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

    public CommonTestFixture()
    {
        var options = new DbContextOptionsBuilder<BankDbContext>().UseInMemoryDatabase(databaseName: "BankDatabaseTest")
            .Options;
        Context = new BankDbContext(options);
        Mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper();

        Context.Database.EnsureCreated();
        Context.AddUsers();
        Context.SaveChanges();
    }
}
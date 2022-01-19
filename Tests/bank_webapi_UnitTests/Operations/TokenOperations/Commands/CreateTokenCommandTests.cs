using System;
using AutoMapper;
using bank_webapi_UnitTests.TestSetup;
using bank_webapi.DbOperations;
using bank_webapi.Entities;
using bank_webapi.Operations.TokenOperations;
using FluentAssertions;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace bank_webapi_UnitTests.Operations.TokenOperations.Commands;

public class CreateTokenCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public CreateTokenCommandTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }
    
    [Fact]
    public void IfLogInInformationIsInvalid_Throw_InvalidOperationException()
    {
        CreateTokenCommand command = new CreateTokenCommand(_context, _mapper, null);
        command.Model = new CreateTokenCommandModel() {Id = 10000, Password = "??????"};
        FluentActions.Invoking(() => command.Handle()).Should()
            .Throw<InvalidOperationException>().And.Message.Should().Be("given informations does not match !!!");
    }
    
    [Fact]
    public void IfLogInInformationIsValid_DoNotThrow_AnyException_ExceptNullParameterError()
    {
        _context.Users.Add(new User()
        {
            Capital = 1, Iban = 12, Investment = 1, Name = "ahmed", Password = "ahmed"
        });
        _context.SaveChanges();
        CreateTokenCommand command = new CreateTokenCommand(_context, _mapper, null);
        command.Model = new CreateTokenCommandModel() {Id = 12, Password = "ahmed"};
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>
            ("_configuration paramater is null");
    }
}
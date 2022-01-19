using System;
using AutoMapper;
using bank_webapi_UnitTests.TestSetup;
using bank_webapi.Controllers;
using bank_webapi.DbOperations;
using bank_webapi.Entities;
using bank_webapi.Operations.UserOperations.Commands;
using FluentAssertions;
using Xunit;

namespace bank_webapi_UnitTests.Operations.UserOperations.Commands;

public class CreateUserCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserCommandTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }
    
    [Theory]
    // negative investment value
    [InlineData("ahmed", "ahmed", -100, -100)]
    // empty name
    [InlineData("", "ahmed", -100, 100)]
    // empty password
    [InlineData("ahmed", "", -100, 100)]
    // short password
    [InlineData("ahmed", "aa", -100, -100)]
    public void IfGivenModelIsInvalid_Throw_ValidatorException(string name, string password, int capital, int investment)
    {
        // authorized person who add new person
        _context.Users.Add(new User()
        {
            AccessToken = "accessgranted", IsBanker = true, Name = "admin2", Password = "admin2"
        });
        _context.SaveChanges();
        CreateUserCommand command = new CreateUserCommand(_context, _mapper, new CreateUserCommandModel()
            {Name = name, Password = password, Capital = capital, Investment = investment}, new UserController.TokenClass()
            {Token = "accessgranted"});
        CreateUserCommandValidator validator = new CreateUserCommandValidator();
        var result = validator.Validate(command);
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void IfGivenModelIsValid_NotThrow_AnyException()
    {
        // authorized person who add new person
        _context.Users.Add(new User()
        {
            AccessToken = "accessgranted", IsBanker = true, Name = "admin2", Password = "admin2"
        });
        _context.SaveChanges();
        CreateUserCommand command = new CreateUserCommand(_context, _mapper, new CreateUserCommandModel()
            {Name = "ahmed", Password = "ahmed", Capital = -100, Investment = +100},
            new UserController.TokenClass(){Token = "accessgranted"});
        CreateUserCommandValidator validator = new CreateUserCommandValidator();
        var result = validator.Validate(command);
        result.Errors.Count.Should().BeLessOrEqualTo(0);
    }

    [Fact]
    public void IfUserIsNotBanker_Throw_UnauthorizedAccessException()
    {
        // not authorized person who try to add new person
        _context.Users.Add(new User()
        {
            AccessToken = "accessnotgranted", IsBanker = false, Name = "admin2", Password = "admin2"
        });
        _context.SaveChanges();
        CreateUserCommand command = new CreateUserCommand(_context, _mapper, new CreateUserCommandModel()
                {Name = "ahmed", Password = "ahmed", Capital = -100, Investment = +100},
            new UserController.TokenClass(){Token = "accessnotgranted"});

        FluentActions.Invoking(() => command.Handle()).Should().Throw<UnauthorizedAccessException>().And
            .Message.Should().Be("user not authorized !!!");

    }
}
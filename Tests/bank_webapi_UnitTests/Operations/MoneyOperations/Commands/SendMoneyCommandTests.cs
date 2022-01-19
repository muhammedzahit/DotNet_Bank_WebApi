using System;
using AutoMapper;
using bank_webapi_UnitTests.TestSetup;
using bank_webapi.DbOperations;
using bank_webapi.Entities;
using bank_webapi.Operations.MoneyOperations.Commands;
using FluentAssertions;
using Xunit;

namespace bank_webapi_UnitTests.Operations.MoneyOperations.Commands;

public class SendMoneyCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public SendMoneyCommandTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Theory]
    // zero amount
    [InlineData(0, "aaa", 10)]
    // negative amount
    [InlineData(-100, "aaa", 10)]
    // empty token
    [InlineData(1, "", 10)]
    // negative or zero iban
    [InlineData(1, "ssssss", -10)]
    [InlineData(1, "ssssss", 0)]
    public void WhenInvalidModelIsGiven_Throw_ValidationError(int amount, string token, int iban)
    {
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = amount, Token = token, Iban = iban
        });
        SendMoneyCommandValidator validator = new SendMoneyCommandValidator();
        var result = validator.Validate(command);
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenInvalidTokenIsGıven_Throw_InvalidOperationError()
    {
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = 10,
            Token = "blabla"
        });
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("token not found !!!");
    }

    [Fact]
    public void WhenValidInputIsGiven_NotThrow_AnyError()
    {
        _context.Users.Add(new User()
        {
            AccessToken = "valid_token_11253",IsBanker = false,Capital = 1000
        });
        _context.SaveChanges();
        
        _context.Users.Add(new User()
        {
            IsBanker = false,Capital = 10,Iban = 122
        });
        _context.SaveChanges();
        
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = 10,
            Token = "valid_token_11253",
            Iban = 122
        });
        FluentActions.Invoking(() => command.Handle()).Should().NotThrow();
    }
    
    [Fact]
    public void WhenUserIsBanker_Throw_InvalidOperationException()
    {
        _context.Users.Add(new User()
        {
            AccessToken = "valid_token_622",IsBanker = true
        });
        _context.SaveChanges();
        
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = 10,
            Token = "valid_token_622"
        });
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("banker users does not have bank accounts !!!");
    }

    [Fact]
    public void WhenInvalidIbanIsGiven_Throw_InvalidOperationException()
    {
        _context.Users.Add(new User()
        {
            AccessToken = "valid_token_2622",IsBanker = false,Capital = 1000
        });
        _context.SaveChanges();
        
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = 10,
            Token = "valid_token_2622",
            Iban = 3333
        });
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("iban not found !!!");
    }

    [Fact]
    public void WhenIbanBelongsToBankerAccount_Throw_InvalidOperationException()
    {
        _context.Users.Add(new User()
        {
            AccessToken = "valid_token_2200",IsBanker = false,Capital = 1000
        });
        _context.SaveChanges();
        
        _context.Users.Add(new User()
        {
            IsBanker = true,Capital = 10,Iban = 122
        });
        _context.SaveChanges();
        
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = 10,
            Token = "valid_token_2200",
            Iban = 122
        });
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("iban belongs to a banker account !!!");
    }
}
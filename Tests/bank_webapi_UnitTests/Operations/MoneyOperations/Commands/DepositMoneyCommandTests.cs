using System;
using AutoMapper;
using bank_webapi_UnitTests.TestSetup;
using bank_webapi.DbOperations;
using bank_webapi.Entities;
using bank_webapi.Operations.MoneyOperations.Commands;
using FluentAssertions;
using Xunit;

namespace bank_webapi_UnitTests.Operations.MoneyOperations.Commands;

public class DepositMoneyCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public DepositMoneyCommandTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Theory]
    // zero amount
    [InlineData(0, "aaa")]
    // negative amount
    [InlineData(-100, "aaa")]
    // empty token
    [InlineData(1, "")]
    public void WhenInvalidModelIsGiven_Throw_ValidationError(int amount, string token)
    {
        DepositMoneyCommand command = new DepositMoneyCommand(_context, _mapper, new DepositMoneyCommandModel()
        {
            Amount = amount, Token = token
        });
        DepositMoneyCommandValidator validator = new DepositMoneyCommandValidator();
        var result = validator.Validate(command);
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenInvalidTokenIsGıven_Throw_InvalidOperationError()
    {
        DepositMoneyCommand command = new DepositMoneyCommand(_context, _mapper, new DepositMoneyCommandModel()
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
            AccessToken = "valid_token_5",IsBanker = false
        });
        _context.SaveChanges();
        
        DepositMoneyCommand command = new DepositMoneyCommand(_context, _mapper, new DepositMoneyCommandModel()
        {
            Amount = 10,
            Token = "valid_token_5"
        });
        FluentActions.Invoking(() => command.Handle()).Should().NotThrow();
    }
    
    [Fact]
    public void WhenUserIsBanker_Throw_InvalidOperationException()
    {
        _context.Users.Add(new User()
        {
            AccessToken = "valid_token_6",IsBanker = true
        });
        _context.SaveChanges();
        
        DepositMoneyCommand command = new DepositMoneyCommand(_context, _mapper, new DepositMoneyCommandModel()
        {
            Amount = 10,
            Token = "valid_token_6"
        });
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("banker users does not have bank accounts !!!");
    }

}
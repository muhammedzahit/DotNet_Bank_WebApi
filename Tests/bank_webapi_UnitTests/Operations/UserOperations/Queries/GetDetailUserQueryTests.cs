using System;
using AutoMapper;
using bank_webapi_UnitTests.TestSetup;
using bank_webapi.Controllers;
using bank_webapi.DbOperations;
using bank_webapi.Entities;
using bank_webapi.Operations.UserOperations.Queries;
using FluentAssertions;
using Xunit;

namespace bank_webapi_UnitTests.Operations.UserOperations.Queries;

public class GetDetailUserQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public GetDetailUserQueryTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void IfUserNotBanker_Throw_UnauthorizedAccessException()
    {
        // banker who search user
        _context.Users.Add(new User()
        {
            AccessToken = "access_not_granted", Name = "client", Password = "client", IsBanker = false
        });
        _context.SaveChanges();
        
        GetDetailUserQuery query = new GetDetailUserQuery(_mapper, _context, new UserController.TokenClass()
        {
            Token = "access_not_granted"
        });
        query.QueryId = 2;

        FluentActions.Invoking(() => query.Handle()).Should().Throw<UnauthorizedAccessException>()
            .And.Message.Should().Be("user not authorized !!!");
    }

    [Fact]
    public void IfSearchedUserIdNotFound_Throw_InvalidOperationException()
    {
        // banker who search user
        _context.Users.Add(new User()
        {
            AccessToken = "access_granted", Name = "admin", Password = "admin", IsBanker = true
        });
        _context.SaveChanges();

        GetDetailUserQuery query = new GetDetailUserQuery(_mapper, _context, new UserController.TokenClass()
        {
            Token = "access_granted"
        });
        query.QueryId = 100;

        FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Given Id is not found");
    }

    [Fact]
    public void IfQueryIsValid_NotThrow_AnyException()
    {
        // banker who search user
        _context.Users.Add(new User()
        {
            AccessToken = "access_granted_3", Name = "admin", Password = "admin", IsBanker = true
        });
        // searched user
        _context.Users.Add(new User()
        {
            Name = "client", Password = "client", Id = 101
        });
        _context.SaveChanges();
        
        GetDetailUserQuery query = new GetDetailUserQuery(_mapper, _context, new UserController.TokenClass()
        {
            Token = "access_granted_3"
        });
        query.QueryId = 101;

        FluentActions.Invoking(() => query.Handle()).Should().NotThrow();
    }

    [Fact]
    public void IfQueryIdIsNegative_Throw_InvalidOperationError()
    {
        // banker who search user
        _context.Users.Add(new User()
        {
            AccessToken = "access_granted", Name = "admin", Password = "admin", IsBanker = true
        });
        
        GetDetailUserQuery query = new GetDetailUserQuery(_mapper, _context, new UserController.TokenClass()
        {
            Token = "access_granted"
        });
        query.QueryId = -100;
        GetDetailUserQueryValidator validator = new GetDetailUserQueryValidator();
        var result = validator.Validate(query);
        
        result.Errors.Count.Should().BeGreaterThan(0);
    }
}
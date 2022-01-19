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

public class GetUsersQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersQueryTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void IfUserNotBanker_Throw_UnauthorizedAccessException()
    {
        _context.Users.Add(new User()
        {
            Name = "ahmed", IsBanker = false, AccessToken = "access_not_granted"
        });
        _context.SaveChanges();
        GetUsersQuery query = new GetUsersQuery(_mapper, _context,
            new UserController.TokenClass() {Token = "access_not_granted"});
        FluentActions.Invoking(() => query.Handle()).Should().Throw<UnauthorizedAccessException>().And
            .Message.Should().Be("user not authorized !!!");
    }

    [Fact]
    public void IfUserBanker_NotThrow_AnyError()
    {
        _context.Users.Add(new User()
        {
            Name = "ahmed", IsBanker = true, AccessToken = "access_granted_2"
        });
        _context.SaveChanges();
        GetUsersQuery query = new GetUsersQuery(_mapper, _context,
            new UserController.TokenClass() {Token = "access_granted_2"});
        FluentActions.Invoking(() => query.Handle()).Should().NotThrow();
    }
}
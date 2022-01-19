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

public class GetMyInfoQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public GetMyInfoQueryTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void WhenAccessTokenIsNotValid_Throw_InvalidOperationError()
    {
        GetMyInfoQuery query =
            new GetMyInfoQuery(_mapper, _context, new UserController.TokenClass() {Token = "blabla"});
        FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("verilen access token gecersiz !!!");
    }

    [Fact]
    public void WhenAccessTokenValid_NotThrow_AnyError()
    {
        _context.Users.Add(new User()
        {
            AccessToken = "valid_token33"
        });
        _context.SaveChanges();
        GetMyInfoQuery query =
            new GetMyInfoQuery(_mapper, _context, new UserController.TokenClass() {Token = "valid_token33"});
        FluentActions.Invoking(() => query.Handle()).Should().NotThrow();
    }
}
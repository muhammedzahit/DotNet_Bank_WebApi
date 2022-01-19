using System;
using AutoMapper;
using bank_webapi_UnitTests.TestSetup;
using bank_webapi.DbOperations;
using bank_webapi.Entities;
using bank_webapi.Operations.TokenOperations;
using FluentAssertions;
using Xunit;

namespace bank_webapi_UnitTests.Operations.TokenOperations.Commands;

public class RefreshTokenCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public RefreshTokenCommandTests(CommonTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void IfGivenTokenIsInvalid_Throw_InvalidOperationException()
    {
        RefreshTokenCommand command = new RefreshTokenCommand(_context, null);
        command.Model = new RefreshTokenCommandModel() {RefreshToken = "thisisnotfoundindatabase"};
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("refresh token did not found !!");
    }

    [Fact]
    public void IfGivenTokenIsExpired_Throw_InvalidOperationException()
    {
        _context.Users.Add(
            new User()
            {
                Capital = 1, Investment = 1, Name = "ahmed", Password = "ahmed",
                RefreshToken = "refreshtoken22", RefreshTokenExpireDate = DateTime.Now.AddHours(-1)
            }
        );
        _context.SaveChanges();
        RefreshTokenCommand command = new RefreshTokenCommand(_context, null);
        command.Model = new RefreshTokenCommandModel() {RefreshToken = "refreshtoken22"};
        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And
            .Message.Should().Be("refresh token expired !!!");
    }

    [Fact]
    public void IfGivenInformationIsValid_NotThrow_AnyException_ExceptNullParameterError()
    {
        _context.Users.Add(
            new User()
            {
                Capital = 1, Investment = 1, Name = "ahmed", Password = "ahmed",
                RefreshToken = "refreshtoken", RefreshTokenExpireDate = DateTime.Now.AddHours(12)
            }
        );
        _context.SaveChanges();
        RefreshTokenCommand command = new RefreshTokenCommand(_context, null);
        command.Model = new RefreshTokenCommandModel() {RefreshToken = "refreshtoken"};
        FluentActions.Invoking(() => command.Handle()).Should().Throw<NullReferenceException>().And
            .Message.Should().Be("_configuration is null !!!");
    }
}
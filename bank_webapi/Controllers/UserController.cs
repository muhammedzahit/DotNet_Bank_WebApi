using System.Collections;
using AutoMapper;
using bank_webapi.DbOperations;
using bank_webapi.Operations.TokenOperations;
using bank_webapi.Operations.UserOperations.Commands;
using bank_webapi.Operations.UserOperations.Queries;
using bank_webapi.TokenHandler;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bank_webapi.Controllers;

[ApiController]
[Route("[controller]s")]
public class UserController : ControllerBase
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    static readonly HttpClient _client = new HttpClient();

    private bool ValidateBanker(string token)
    {
        var user = _context.Users.SingleOrDefault(x => x.AccessToken == token && x.IsBanker == true);
        if (user is null)
            return false;
        return true;
    }
    
    public class TokenClass
    {
        public string Token { get; set; }
    }

    public class CreateUserBody
    {
        public TokenClass Token { get; set; }
        public CreateUserCommandModel Model { get; set; }
    }
    
    public UserController(IBankDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetUsers([FromBody] TokenClass token)
    {
        if (!ValidateBanker(token.Token))
            return BadRequest("User not authorized for this operation");

        GetUsersQuery query = new GetUsersQuery(_mapper, _context);
        var users = query.Handle();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetDetailUser([FromBody] TokenClass token, string id)
    {
        if (!ValidateBanker(token.Token))
            return BadRequest("User not authorized for this operation");

        GetDetailUserQuery query = new GetDetailUserQuery(_mapper, _context);
        query.QueryId = Convert.ToInt32(id);
        GetDetailUserQueryModel model;
        try
        {
            GetDetailUserQueryValidator validator = new GetDetailUserQueryValidator();
            validator.ValidateAndThrow(query);
            model = query.Handle();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }

        return Ok(model);
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserBody body)
    {
        Console.WriteLine(body.Token.Token);
        if (!ValidateBanker(body.Token.Token))
            return BadRequest("User not authorized for this operation");
        
        CreateUserCommand command = new CreateUserCommand(_context, _mapper, body.Model);
        CreateUserCommandValidator validator = new CreateUserCommandValidator();
        try
        {
            validator.ValidateAndThrow(command);
            command.Handle();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }

        return Ok();
    }
    
    [Authorize]
    [HttpGet("my_info")]
    public IActionResult GetMyInfo([FromBody] TokenClass tokenClass)
    {
        var user = _context.Users.SingleOrDefault(x => x.AccessToken.Equals(tokenClass.Token));

        if (user is null)
            BadRequest("verilen access token gecersiz !!!");
        
        GetMyInfoQuery query = new GetMyInfoQuery(_mapper, _context);
        query.QueryId = Convert.ToInt32(user.Id);
        GetMyInfoQueryModel model;
        try
        {
            model = query.Handle();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }

        return Ok(model);
    }

    [HttpPost("token")]
    public IActionResult CreateToken([FromBody] CreateTokenCommandModel model)
    {
        CreateTokenCommand command = new CreateTokenCommand(_context, _mapper, _configuration);
        command.Model = model;
        Token token;
        try
        {
            token = command.Handle();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }

        return Ok(token);
    }

    [HttpPost("refresh_token")]
    public IActionResult RefreshToken(RefreshTokenCommandModel model)
    {
        RefreshTokenCommand command = new RefreshTokenCommand(_context, _configuration);
        command.Model = model;
        Token token = null;
        try
        {
            token = command.Handle();
        }
        catch (Exception e)
        {
            BadRequest(e.ToString());
        }

        return Ok(token);
    }
}
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
        List<GetUsersQueryModel> users;
        try
        {
            GetUsersQuery query = new GetUsersQuery(_mapper, _context, token);
            users = query.Handle();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetDetailUser([FromBody] TokenClass token, string id)
    {
        GetDetailUserQuery query = new GetDetailUserQuery(_mapper, _context, token);
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
        CreateUserCommand command = new CreateUserCommand(_context, _mapper, body.Model, body.Token);
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
        
        
        GetMyInfoQuery query = new GetMyInfoQuery(_mapper, _context, tokenClass);
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
using AutoMapper;
using bank_webapi.DbOperations;
using bank_webapi.Operations.MoneyOperations.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bank_webapi.Controllers;

[ApiController]
[Route("Money")]
public class MoneyController : ControllerBase
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public MoneyController(IBankDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    [Authorize]
    [HttpPost("deposit")]
    public IActionResult DepositMoney(DepositMoneyCommandModel model)
    {
        DepositMoneyCommand command = new DepositMoneyCommand(_context, _mapper, model);
        DepositMoneyCommandValidator validator = new DepositMoneyCommandValidator();
        try
        {
            validator.ValidateAndThrow(command);
            command.Handle();
        }
        catch (Exception e)
        {
            BadRequest(e.ToString());
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("withdraw")]
    public IActionResult WithdrawMoney(WithdrawMoneyCommandModel model)
    {
        WithdrawMoneyCommand command = new WithdrawMoneyCommand(_context, _mapper, model);
        WithdrawMoneyCommandValidator validator = new WithdrawMoneyCommandValidator();
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
    [HttpPost("invest")]
    public IActionResult InvestMoney(InvestMoneyCommandModel model)
    {
        InvestMoneyCommand command = new InvestMoneyCommand(_context, _mapper, new InvestMoneyCommandModel()
        {
            Amount = model.Amount, Token = model.Token
        });
        InvestMoneyCommandValidator validator = new InvestMoneyCommandValidator();
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
    [HttpPost("send")]
    public IActionResult InvestMoney(SendMoneyCommandModel model)
    {
        SendMoneyCommand command = new SendMoneyCommand(_context, _mapper, new SendMoneyCommandModel()
        {
            Amount = model.Amount, Token = model.Token, Iban = model.Iban
        });
        SendMoneyCommandValidator validator = new SendMoneyCommandValidator();
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
}
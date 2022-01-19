using AutoMapper;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class DepositMoneyCommand
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    public DepositMoneyCommandModel Model { get; set; }
    
    public DepositMoneyCommand(IBankDbContext context, IMapper mapper, DepositMoneyCommandModel model)
    {
        _context = context;
        _mapper = mapper;
        Model = model;
    }

    public void Handle()
    {
        var user = _context.Users.SingleOrDefault(x => x.AccessToken == Model.Token);
        if (user is null)
            throw new InvalidOperationException("token not found !!!");

        if (!Helper.isClient(_context, Model.Token))
            throw new InvalidOperationException("banker users does not have bank accounts !!!");
        
        user.Capital += Model.Amount;
        _context.SaveChanges();
    }
    
}

public class DepositMoneyCommandModel
{
    public int Amount { get; set; }
    public string Token { get; set; } = "";
}
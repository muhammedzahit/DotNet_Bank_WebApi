using AutoMapper;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class WithdrawMoneyCommand
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    public WithdrawMoneyCommandModel Model { get; set; }

    public WithdrawMoneyCommand(IBankDbContext context, IMapper mapper, WithdrawMoneyCommandModel model)
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
        
        if (Model.Amount > user.Capital)
            throw new InvalidOperationException("this amount greater than your capital !!!");
        
        user.Capital -= Model.Amount;
        _context.SaveChanges();
    }
}

public class WithdrawMoneyCommandModel
{
    public int Amount { get; set; }
    public string Token { get; set; } = "";
}
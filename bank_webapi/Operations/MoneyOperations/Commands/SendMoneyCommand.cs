using AutoMapper;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class SendMoneyCommand
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    public SendMoneyCommandModel Model { get; set; }
    
    public SendMoneyCommand(IBankDbContext context, IMapper mapper, SendMoneyCommandModel model)
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

        var user2 = _context.Users.SingleOrDefault(x => x.Iban == Model.Iban);
        if (user2 is null)
            throw new InvalidOperationException("iban not found !!!");
        else if (user2.IsBanker)
            throw new InvalidOperationException("iban belongs to a banker account !!!");

        user.Capital -= Model.Amount;
        user2.Capital += Model.Amount;
        
        _context.SaveChanges();
    }
}

public class SendMoneyCommandModel
{
    public int Amount { get; set; }
    public string Token { get; set; } = "";
    public int Iban { get; set; }
}
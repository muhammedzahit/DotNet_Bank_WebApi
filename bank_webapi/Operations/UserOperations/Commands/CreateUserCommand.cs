using System.Security.Cryptography;
using AutoMapper;
using bank_webapi.DbOperations;
using bank_webapi.Entities;

namespace bank_webapi.Operations.UserOperations.Commands;

public class CreateUserCommand
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    private readonly Random _random = new Random();

    public CreateUserCommandModel Model { get; set; }
    
    public CreateUserCommand(IBankDbContext context, IMapper mapper, CreateUserCommandModel model)
    {
        _context = context;
        _mapper = mapper;
        Model = model;
    }

    public void Handle()
    {
        int ibanRandom = _random.Next(0, 1000);
        var check = _context.Users.SingleOrDefault(x => x.Iban == ibanRandom);
        while (check is not null)
        {
            ibanRandom = _random.Next(0, 1000);
            check = _context.Users.SingleOrDefault(x => x.Iban == ibanRandom);
        }

        User user = _mapper.Map<User>(Model);
        user.Iban = ibanRandom;
        user.IsBanker = false;
        _context.Users.Add(user);
        _context.SaveChanges();

    }
}

public class CreateUserCommandModel
{
    public string Name { get; set; } = "";
    
    public string Password { get; set; } = "";

    public int Capital { get; set; }
    
    public int Investment { get; set; }
}
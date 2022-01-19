using System.Linq;
using AutoMapper;
using bank_webapi.Controllers;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetMyInfoQuery
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    
    private UserController.TokenClass _TokenClass { get; set; }
    
    public GetMyInfoQuery(IMapper mapper, IBankDbContext context, UserController.TokenClass tokenClass)
    {
        _mapper = mapper;
        _context = context;
        _TokenClass = tokenClass;
    }

    public GetMyInfoQueryModel Handle()
    {
        var user = _context.Users.SingleOrDefault(x => x.AccessToken.Equals(_TokenClass.Token));

        if (user is null)
            throw new InvalidOperationException("verilen access token gecersiz !!!");
        
        return _mapper.Map<GetMyInfoQueryModel>(user);
    }
}

public class GetMyInfoQueryModel
{
    public int Iban { get; set; }
    
    public string Name { get; set; } = "";

    public bool IsBanker { get; set; }
    
    public string Password { get; set; } = "";

    public int Capital { get; set; }
    
    public int Investment { get; set; }
}
using AutoMapper;
using bank_webapi.Controllers;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetDetailUserQuery
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserController.TokenClass _tokenClass;

    public int QueryId { get; set; }
    
    private bool ValidateBanker(string token)
    {
        var user = _context.Users.SingleOrDefault(x => x.AccessToken == token && x.IsBanker == true);
        if (user is null)
            return false;
        return true;
    }
    
    public GetDetailUserQuery(IMapper mapper, IBankDbContext context, UserController.TokenClass tokenClass)
    {
        _mapper = mapper;
        _context = context;
        _tokenClass = tokenClass;
    }

    public GetDetailUserQueryModel Handle()
    {
        if (!Helpers.ValidateBanker(_tokenClass.Token, _context))
            throw new UnauthorizedAccessException("user not authorized !!!");
        
        var user = _context.Users.SingleOrDefault(x => x.Id == QueryId);
        if (user is null)
            throw new InvalidOperationException("Given Id is not found");
        return _mapper.Map<GetDetailUserQueryModel>(user);
    }
}

public class GetDetailUserQueryModel
{
    public string Name { get; set; } = "";
    
    public int Iban { get; set; }

    public bool IsBanker { get; set; }
    
    public int Capital { get; set; }
    
    public int Investment { get; set; }
    
}
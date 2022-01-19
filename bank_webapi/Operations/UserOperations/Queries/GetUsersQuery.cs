using AutoMapper;
using bank_webapi.Controllers;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetUsersQuery
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserController.TokenClass _tokenClass;

    public GetUsersQuery(IMapper mapper, IBankDbContext context, UserController.TokenClass tokenClass)
    {
        _mapper = mapper;
        _context = context;
        _tokenClass = tokenClass;
    }

    public List<GetUsersQueryModel> Handle()
    {
        if (!Helpers.ValidateBanker(_tokenClass.Token, _context))
            throw new UnauthorizedAccessException("user not authorized !!!");
        var users = _context.Users.ToList();
        var models = _mapper.Map<List<GetUsersQueryModel>>(users);
        return models;
    }
}

public class GetUsersQueryModel
{
    public string Name { get; set; } = "";
    public bool IsBanker { get; set; }
    public int Id { get; set; }
    public int Iban { get; set; }
}
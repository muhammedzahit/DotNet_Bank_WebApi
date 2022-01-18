using AutoMapper;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetUsersQuery
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersQuery(IMapper mapper, IBankDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public List<GetUsersQueryModel> Handle()
    {
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
using AutoMapper;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetDetailUserQuery
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public int QueryId { get; set; }
    
    public GetDetailUserQuery(IMapper mapper, IBankDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public GetDetailUserQueryModel Handle()
    {
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
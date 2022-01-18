using AutoMapper;
using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetMyInfoQuery
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;

    public int QueryId { get; set; }
    
    public GetMyInfoQuery(IMapper mapper, IBankDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public GetMyInfoQueryModel Handle()
    {
        var user = _context.Users.SingleOrDefault(x => x.Id == QueryId);
        if (user is null)
            throw new InvalidOperationException("Given Id is not found");
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
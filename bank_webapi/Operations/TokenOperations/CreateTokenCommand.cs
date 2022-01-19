using AutoMapper;
using bank_webapi.DbOperations;
using bank_webapi.TokenHandler;

namespace bank_webapi.Operations.TokenOperations;

public class CreateTokenCommand
{
    private readonly IBankDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    
    public CreateTokenCommandModel Model { get; set; }
    
    public CreateTokenCommand(IBankDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public Token Handle()
    {
        var user = _context.Users.SingleOrDefault(x => x.Id == Model.Id && x.Password == Model.Password);
        if (user is null)
            throw new InvalidOperationException("given informations does not match !!!");
        if (_configuration is null)
            throw new InvalidOperationException("_configuration paramater is null");
        TokenHandler.TokenHandler handler = new TokenHandler.TokenHandler(_context, _configuration);
        Token token = handler.CreateToken();
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpireDate = DateTime.Now.AddMinutes(30);
        user.AccessToken = token.AccessToken;
        _context.SaveChanges();
        return token;
    }
}

public class CreateTokenCommandModel
{
    public int Id { get; set; }
    public string Password { get; set; } = "";
}
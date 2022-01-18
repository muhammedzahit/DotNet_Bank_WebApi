using bank_webapi.DbOperations;
using bank_webapi.TokenHandler;

namespace bank_webapi.Operations.TokenOperations;

public class RefreshTokenCommand
{
    private readonly IBankDbContext _context;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandModel Model { get; set; }
    
    public RefreshTokenCommand(IBankDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public Token Handle()
    {
        var user = _context.Users.SingleOrDefault(x => x.RefreshToken == Model.RefreshToken);
        if (user is null)
            throw new InvalidOperationException("refresh token did not found !!");
        else if (user.RefreshTokenExpireDate < DateTime.Now)
            throw new InvalidOperationException("refresh token expired !!!");
        TokenHandler.TokenHandler handler = new TokenHandler.TokenHandler(_context, _configuration);
        Token token = handler.CreateToken();
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpireDate = DateTime.Now.AddMinutes(30);
        user.AccessToken = token.AccessToken;
        _context.SaveChanges();
        return token;
    }
}

public class RefreshTokenCommandModel
{
    public string RefreshToken { get; set; }
}
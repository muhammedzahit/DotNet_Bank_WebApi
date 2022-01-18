using System.IdentityModel.Tokens.Jwt;
using System.Text;
using bank_webapi.DbOperations;
using Microsoft.IdentityModel.Tokens;

namespace bank_webapi.TokenHandler;

public class TokenHandler
{
    private readonly IBankDbContext _context;
    private readonly IConfiguration _configuration;

    public TokenHandler(IBankDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public Token CreateToken()
    {
        Token token = new Token();
        SymmetricSecurityKey symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]))
            ;
        SigningCredentials credentials =
            new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        token.RefreshTokenExpireDate = DateTime.Now.AddMinutes(15);

        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer:_configuration["Token:Issuer"],
            audience:_configuration["Token:Audience"],
            expires: token.RefreshTokenExpireDate,
            notBefore: DateTime.Now,
            signingCredentials: credentials
        );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        token.AccessToken = handler.WriteToken(securityToken);
        token.RefreshToken = Guid.NewGuid().ToString();
        return token;
    }
}
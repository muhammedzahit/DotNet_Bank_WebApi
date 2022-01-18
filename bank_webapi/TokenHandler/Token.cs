namespace bank_webapi.TokenHandler;

public class Token
{
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public DateTime RefreshTokenExpireDate { get; set; }
}
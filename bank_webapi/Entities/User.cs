using System.ComponentModel.DataAnnotations.Schema;

namespace bank_webapi.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int Iban { get; set; }
    
    public string Name { get; set; } = "";

    public bool IsBanker { get; set; }
    public string Password { get; set; } = "";

    public string RefreshToken { get; set; } = "";

    public string AccessToken { get; set; } = "";
    
    public int Capital { get; set; }
    
    public int Investment { get; set; }
    
    public DateTime RefreshTokenExpireDate { get; set; }
}
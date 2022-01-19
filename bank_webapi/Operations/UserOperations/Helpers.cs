using bank_webapi.DbOperations;

namespace bank_webapi.Operations.UserOperations;

public static class Helpers
{
    public static bool ValidateBanker(string token, IBankDbContext _context)
    {
        var user = _context.Users.SingleOrDefault(x => x.AccessToken == token && x.IsBanker == true);
        if (user is null)
            return false;
        return true;
    }
}
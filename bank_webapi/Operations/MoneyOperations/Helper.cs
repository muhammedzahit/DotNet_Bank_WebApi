using bank_webapi.DbOperations;

namespace bank_webapi.Operations.MoneyOperations;

public static class Helper
{
    public static bool isClient(IBankDbContext context, string token)
    {
        var user = context.Users.SingleOrDefault(x => x.AccessToken == token);
        if (user.IsBanker)
            return false;
        return true;
    }
}
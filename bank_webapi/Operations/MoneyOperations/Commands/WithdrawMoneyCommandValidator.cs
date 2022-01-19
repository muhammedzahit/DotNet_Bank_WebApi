using FluentValidation;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class WithdrawMoneyCommandValidator : AbstractValidator<WithdrawMoneyCommand>
{
    public WithdrawMoneyCommandValidator()
    {
        RuleFor(x => x.Model.Amount).GreaterThan(0);
        RuleFor(x => x.Model.Token).NotEmpty();
    }
}
using FluentValidation;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class DepositMoneyCommandValidator : AbstractValidator<DepositMoneyCommand>
{
    public DepositMoneyCommandValidator()
    {
        RuleFor(x => x.Model.Amount).GreaterThan(0);
        RuleFor(x => x.Model.Token).NotEmpty();
    }
}
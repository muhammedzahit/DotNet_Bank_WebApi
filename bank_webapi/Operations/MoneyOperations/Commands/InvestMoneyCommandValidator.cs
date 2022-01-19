using FluentValidation;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class InvestMoneyCommandValidator : AbstractValidator<InvestMoneyCommand>
{
    public InvestMoneyCommandValidator()
    {
        RuleFor(x => x.Model.Amount).GreaterThan(0);
        RuleFor(x => x.Model.Token).NotEmpty();
    }
}
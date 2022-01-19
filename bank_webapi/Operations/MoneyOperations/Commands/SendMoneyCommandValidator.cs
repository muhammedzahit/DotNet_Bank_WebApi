using FluentValidation;

namespace bank_webapi.Operations.MoneyOperations.Commands;

public class SendMoneyCommandValidator : AbstractValidator<SendMoneyCommand>
{
    public SendMoneyCommandValidator()
    {
        RuleFor(x => x.Model.Amount).GreaterThan(0);
        RuleFor(x => x.Model.Token).NotEmpty();
        RuleFor(x => x.Model.Iban).GreaterThan(0);
    }
}
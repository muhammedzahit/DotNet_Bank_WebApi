using FluentValidation;

namespace bank_webapi.Operations.UserOperations.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Model.Investment).GreaterThan(-1).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty();
        RuleFor(x => x.Model.Password).NotEmpty();
        RuleFor(x => x.Model.Password).Must(x => x.Length > 4);

    }
}
using FluentValidation;

namespace bank_webapi.Operations.UserOperations.Queries;

public class GetDetailUserQueryValidator : AbstractValidator<GetDetailUserQuery>
{
    public GetDetailUserQueryValidator()
    {
        RuleFor(command => command.QueryId).GreaterThan(0);
    }
}
using FluentValidation;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Person.Queries;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Validators;

public class GetPersonsQueryValidator : AbstractValidator<GetPersonsQuery>
{
    public GetPersonsQueryValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.Order)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.Order))
            .MustBeAValidEnum<GetPersonsQuery, PersonSortBy>();

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be a positive number.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize must be a positive number");

        RuleFor(x => x.UserId)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.UserId))
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}
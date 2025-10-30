using FluentValidation;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Activity.Queries;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Activity.Validators;

public class GetActivitiesQueryValidator : AbstractValidator<GetActivitiesQuery>
{
    public GetActivitiesQueryValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.PersonId))
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetActivitiesQuery, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.Order)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.Order))
            .MustBeAValidEnum<GetActivitiesQuery, ActivitySortBy>();

        RuleFor(x => x.UserId)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.UserId))
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
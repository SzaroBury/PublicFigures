using FluentValidation;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Activity.Queries;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Activity.Validators;

public class GetActivitiesQueryValidator : AbstractValidator<GetActivitiesQuery>
{
    public GetActivitiesQueryValidator(IReadOnlyRepository repository, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetActivitiesQuery, Domain.Model.Person>(repository)
            .When(x => !string.IsNullOrWhiteSpace(x.PersonId));

        RuleFor(x => x.Order)
            .MustBeAValidEnum<GetActivitiesQuery, ActivitySortBy>()
            .When(x => !string.IsNullOrWhiteSpace(x.Order));

        RuleFor(x => x.UserId)
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repository, identityService)
            .When(x => !string.IsNullOrWhiteSpace(x.UserId));
    }
}
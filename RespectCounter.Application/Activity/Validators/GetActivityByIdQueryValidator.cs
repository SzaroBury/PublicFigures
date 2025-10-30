using FluentValidation;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Activity.Queries;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Activity.Validators;

public class GetActivityByIdQueryValidator : AbstractValidator<GetActivityByIdQuery>
{
    public GetActivityByIdQueryValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetActivityByIdQuery, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.UserId)
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
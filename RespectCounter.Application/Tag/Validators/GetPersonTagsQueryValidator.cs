using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Tags.Queries;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Tags.Validators;

public class GetPersonTagsQueryValidator : AbstractValidator<GetPersonTagsQuery>
{
    public GetPersonTagsQueryValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetPersonTagsQuery, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.AtLeastCount)
            .GreaterThanOrEqualTo(0).WithMessage("AtLeastCount mus be a postive number.");
    }
}
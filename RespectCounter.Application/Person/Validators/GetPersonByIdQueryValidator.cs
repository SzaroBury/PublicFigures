using FluentValidation;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Person.Queries;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Validators;

public class GetPersonByIdQueryValidator : AbstractValidator<GetPersonByIdQuery>
{
    public GetPersonByIdQueryValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetPersonByIdQuery, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.UserId)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.UserId))
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
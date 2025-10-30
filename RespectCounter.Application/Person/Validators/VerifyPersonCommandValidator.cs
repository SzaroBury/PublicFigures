using FluentValidation;
using RespectCounter.Application.Person.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Person.Validators;

public class VerifyPersonCommandValidator : AbstractValidator<VerifyPersonCommand>
{
    public VerifyPersonCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<VerifyPersonCommand, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.UserId)
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
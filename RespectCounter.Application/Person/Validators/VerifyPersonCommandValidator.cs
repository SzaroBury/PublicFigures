using FluentValidation;
using RespectCounter.Application.Person.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Person.Validators;

public class VerifyPersonCommandValidator : AbstractValidator<VerifyPersonCommand>
{
    public VerifyPersonCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<VerifyPersonCommand, Domain.Model.Person>(repo);

        RuleFor(x => x.UserId)
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}
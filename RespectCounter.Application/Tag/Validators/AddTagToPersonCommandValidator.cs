using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Tags.Commands;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Tags.Validators;

public class AddTagToPersonCommandValidator : AbstractValidator<AddTagToPersonCommand>
{
    public AddTagToPersonCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddTagToPersonCommand, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.TagName)
            .NotEmpty().WithMessage("TagName is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
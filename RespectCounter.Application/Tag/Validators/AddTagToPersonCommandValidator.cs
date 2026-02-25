using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Tag.Commands;

namespace RespectCounter.Application.Tag.Validators;

public class AddTagToPersonCommandValidator : AbstractValidator<AddTagToPersonCommand>
{
    public AddTagToPersonCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddTagToPersonCommand, Domain.Model.Person>(repo);

        RuleFor(x => x.TagName)
            .NotEmpty().WithMessage("TagName is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}
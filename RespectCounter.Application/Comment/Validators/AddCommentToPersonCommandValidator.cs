using FluentValidation;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Comment.Validators;

public class AddCommentToPersonCommentCommandValidator : AbstractValidator<AddCommentToPersonCommand>
{
    public AddCommentToPersonCommentCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddCommentToPersonCommand, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
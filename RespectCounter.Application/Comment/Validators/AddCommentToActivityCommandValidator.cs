using FluentValidation;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Comment.Validators;

public class AddCommentToActivityCommentCommandValidator : AbstractValidator<AddCommentToActivityCommand>
{
    public AddCommentToActivityCommentCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddCommentToActivityCommand, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
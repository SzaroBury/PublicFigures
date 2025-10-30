using FluentValidation;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Comment.Validators;

public class HideCommentCommandValidator : AbstractValidator<HideCommentCommand>
{
    public HideCommentCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("CommentId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<HideCommentCommand, Domain.Model.Comment>(entityChecker);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
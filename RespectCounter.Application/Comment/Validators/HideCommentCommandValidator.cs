using FluentValidation;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Comment.Validators;

public class HideCommentCommandValidator : AbstractValidator<HideCommentCommand>
{
    public HideCommentCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("CommentId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<HideCommentCommand, Domain.Model.Comment>(repo);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}
using FluentValidation;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Reactions.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Reactions.Validators;

public class AddReactionToCommentCommandValidator : AbstractValidator<AddReactionToCommentCommand>
{
    public AddReactionToCommentCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("CommentId is required")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddReactionToCommentCommand, Domain.Model.Activity>(repo);

        RuleFor(x => x.ReactionType)
            .NotEmpty().WithMessage("ReactionType is required")
            .MustBeAValidEnum<AddReactionToCommentCommand, ReactionType>();

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}
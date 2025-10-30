using FluentValidation;
using RespectCounter.Application.Reactions.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Reactions.Validators;

public class AddReactionToCommentCommandValidator : AbstractValidator<AddReactionToCommentCommand>
{
    public AddReactionToCommentCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("CommentId is required")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddReactionToCommentCommand, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.ReactionType)
            .NotEmpty().WithMessage("ReactionType is required")
            .MustBeAValidEnum<AddReactionToCommentCommand, ReactionType>();

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
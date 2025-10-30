using FluentValidation;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Comment.Validators;

public class AddCommentToParentCommentCommandValidator : AbstractValidator<AddCommentToParentCommentCommand>
{
    public AddCommentToParentCommentCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.ParentCommentId)
            .NotEmpty().WithMessage("ParentCommentId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddCommentToParentCommentCommand, Domain.Model.Comment>(entityChecker);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
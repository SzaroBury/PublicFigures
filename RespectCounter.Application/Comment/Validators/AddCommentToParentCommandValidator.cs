using FluentValidation;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Comment.Validators;

public class AddCommentToParentCommentCommandValidator : AbstractValidator<AddCommentToParentCommentCommand>
{
    public AddCommentToParentCommentCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.ParentCommentId)
            .NotEmpty().WithMessage("ParentCommentId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddCommentToParentCommentCommand, Domain.Model.Comment>(repo);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}
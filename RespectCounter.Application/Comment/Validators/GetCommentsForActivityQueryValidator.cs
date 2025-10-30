using FluentValidation;
using RespectCounter.Application.Comment.Queries;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Comment.Validators;

public class GetCommentsForActivityQueryValidator : AbstractValidator<GetCommentsForActivityQuery>
{
    public GetCommentsForActivityQueryValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetCommentsForActivityQuery, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be 1 or greater.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.Levels)
            .GreaterThanOrEqualTo(0).WithMessage("Levels must be 0 or greater.")
            .LessThanOrEqualTo(5).WithMessage("The maximum nesting level for comments is 5.");

        RuleFor(x => x.Order)
            .Must(x=> true)
            .When(x => !string.IsNullOrWhiteSpace(x.Order))
            .MustBeAValidEnum<GetCommentsForActivityQuery, CommentSortBy>();

        RuleFor(x => x.UserId)
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}
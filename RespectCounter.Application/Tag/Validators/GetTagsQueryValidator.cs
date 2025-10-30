using FluentValidation;
using RespectCounter.Application.Tags.Queries;

namespace RespectCounter.Application.Tags.Validators;

public class GetTagsQueryValidator : AbstractValidator<GetTagsQuery>
{
    public GetTagsQueryValidator()
    {
        RuleFor(x => x.AtLeastCount)
                .GreaterThanOrEqualTo(0).WithMessage("AtLeastCount mus be a postive number.");
    }
}
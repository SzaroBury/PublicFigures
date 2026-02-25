using FluentValidation;
using RespectCounter.Application.Tag.Queries;

namespace RespectCounter.Application.Tag.Validators;

public class GetTagsQueryValidator : AbstractValidator<GetTagsQuery>
{
    public GetTagsQueryValidator()
    {
        RuleFor(x => x.AtLeastCount)
                .GreaterThanOrEqualTo(0).WithMessage("AtLeastCount mus be a postive number.");
    }
}
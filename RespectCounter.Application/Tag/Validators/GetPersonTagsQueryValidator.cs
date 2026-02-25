using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Tag.Queries;

namespace RespectCounter.Application.Tag.Validators;

public class GetPersonTagsQueryValidator : AbstractValidator<GetPersonTagsQuery>
{
    public GetPersonTagsQueryValidator(IReadOnlyRepository repo)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<GetPersonTagsQuery, Domain.Model.Person>(repo);

        RuleFor(x => x.AtLeastCount)
            .GreaterThanOrEqualTo(0).WithMessage("AtLeastCount mus be a postive number.");
    }
}
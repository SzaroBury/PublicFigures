using FluentValidation;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Activity.Validators;

public class AddActivityCommandValidator : AbstractValidator<AddActivityCommand>
{
    public AddActivityCommandValidator(IReadOnlyRepository repository, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddActivityCommand, Domain.Model.Person>(repository);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repository, identityService);

        RuleFor(x => x.OccurredAt)
            .MustBeAValidDate()
            .When(x => !string.IsNullOrWhiteSpace(x.OccurredAt));

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MustBeAValidEnum<AddActivityCommand, ActivityType>();
    }
}
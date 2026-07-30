using FluentValidation;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Activity.Validators;

public class AddActivityCommandValidator : AbstractValidator<AddActivityCommand>
{
    private const string requiredMessage = "'{PropertyName}' is required.";

    public AddActivityCommandValidator(IReadOnlyRepository repository, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage(requiredMessage)
            .MustBeAValidGuid()
            .WithName("personId")
            .MustBeAnExistingEntityAsync<AddActivityCommand, Domain.Model.Person>(repository);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(requiredMessage)
            .MustBeAValidGuid()
            .WithName("userId")
            .MustBeAnExistingUserAsync(repository, identityService);

        RuleFor(x => x.OccurredAt)
            .MustBeAValidDate()
            .WithName("occurredAt")
            .When(x => !string.IsNullOrWhiteSpace(x.OccurredAt));

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(requiredMessage)
            .WithName("type")
            .MustBeAValidEnum<AddActivityCommand, ActivityType>();
    }
}
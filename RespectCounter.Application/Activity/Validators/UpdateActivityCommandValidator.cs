using FluentValidation;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Activity.Validators;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
{
    public UpdateActivityCommandValidator(IReadOnlyRepository repository, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<UpdateActivityCommand, Domain.Model.Activity>(repository);

        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<UpdateActivityCommand, Domain.Model.Person>(repository);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repository, identityService);

        RuleFor(x => x.OccurredAt)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.OccurredAt))
            .MustBeAValidDate();

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MustBeAValidEnum<UpdateActivityCommand, ActivityType>();
    }
}
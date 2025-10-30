using FluentValidation;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Activity.Validators;

public class AddActivityCommandValidator : AbstractValidator<AddActivityCommand>
{
    private readonly IEntityChecker _entityChecker;

    public AddActivityCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        _entityChecker = entityChecker;

        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddActivityCommand, Domain.Model.Person>(entityChecker);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);

        RuleFor(x => x.Happend)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.Happend))
            .MustBeAValidDate();

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MustBeAValidEnum<AddActivityCommand, ActivityType>();
    }
}
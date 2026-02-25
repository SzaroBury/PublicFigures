using FluentValidation;
using RespectCounter.Application.Person.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Person.Validators;

public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
{
    public AddPersonCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The first name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            
        RuleFor(x => x.NickName)
            .MaximumLength(50).WithMessage("Nickname cannot exceed 50 characters.");
            
        RuleFor(x => x.Profession)
            .NotEmpty().WithMessage("The profession is required.")
            .MaximumLength(100).WithMessage("Profession cannot exceed 100 characters.");
            
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage("Nationality is required.")
            .MaximumLength(50).WithMessage("Nationality cannot exceed 50 characters.");
            
        RuleFor(x => x.Tags)
            .NotEmpty().WithMessage("At least one tag is required.");

        RuleFor(x => x.Birthday)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.Birthday))
            .MustBeAValidDateOnly();

        RuleFor(x => x.DeathDate)
            .Must(x => true)
            .When(x => !string.IsNullOrWhiteSpace(x.Birthday))
            .MustBeAValidDateOnly()
            .Must((command, deathDate) => DateAfterBirthday(command.Birthday, deathDate))
                .WithMessage("The Death Date cannot be before the Birthday.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }

    private bool DateAfterBirthday(string? birthday, string? deathDate)
    {
        if (string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(deathDate))
        {
            return false;
        }

        if (birthday.ToDateOnly() is DateOnly bd && deathDate.ToDateOnly() is DateOnly dd)
        {
            return dd.CompareTo(bd) >= 0;
        }
        return false;
    }
}
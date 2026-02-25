using System.Globalization;
using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Shared.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> MustBeAValidGuid<T>(this IRuleBuilder<T, string?> builder)
    {
        return builder.Must(id =>
        {
            return Guid.TryParse(id, out _);
        })
        .WithMessage("{PropertyName} must be a valid GUID format.");
    }

    public static IRuleBuilderOptions<T, string?> MustBeAValidDate<T>(this IRuleBuilder<T, string?> builder)
    {
        return builder.Must(dateString =>
        {
            return DateTime.TryParseExact(
                dateString,
                "yyyy-MM-ddTHH:mm:ss.fffZ",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out DateTime parsedDate);
        })
        .WithMessage("Invalid date format for field '{PropertyName}'. Expected format: yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    public static IRuleBuilderOptions<T, string?> MustBeAValidDateOnly<T>(this IRuleBuilder<T, string?> builder)
    {
        return builder.Must(dateString =>
        {
            return DateOnly.TryParseExact(
                dateString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out DateOnly parsedDate);
        })
        .WithMessage("Invalid date format for field '{PropertyName}'. Expected format: yyyy-MM-dd");
    }

    public static IRuleBuilderOptions<T, int> MustBeAValidEnum<T, TEnum>(this IRuleBuilder<T, int> builder)
    {
        return builder.Must((entity, enumValue, context) =>
        {
            context.MessageFormatter.AppendArgument("EnumType", typeof(TEnum).Name);
            return Enum.IsDefined(typeof(TEnum), enumValue);
        })
        .WithMessage("{PropertyName} must be a valid value for {EnumType}.");
    }

    public static IRuleBuilderOptions<T, string?> MustBeAValidEnum<T, TEnum>(this IRuleBuilder<T, string?> builder)
    {
        return builder.Must((entity, enumValue, context) =>
        {
            if (string.IsNullOrWhiteSpace(enumValue))
            {
                return false;
            }

            context.MessageFormatter.AppendArgument("EnumType", nameof(TEnum));
            return Enum.IsDefined(typeof(TEnum), enumValue);
        })
        .WithMessage("{PropertyName} must be a valid value for {EnumType}.");
    }

    public static IRuleBuilderOptions<T, string?> MustBeAnExistingUserAsync<T>(
        this IRuleBuilder<T, string?> builder,
        IReadOnlyRepository repository,
        IIdentityService identityService)
    {
        builder = builder.MustAsync(async (id, cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var userGuid))
            {
                return false;
            }

            return await repository.ExistsAsync<User>(userGuid, cancellationToken);
        })
        .WithMessage("User profile with the ID '{PropertyValue}' not found.");

        return builder.MustAsync(async (id, cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var identityGuid))
            {
                return false;
            }

            return await identityService.ExistsAsync(identityGuid, cancellationToken);
        })
        .WithMessage("User identity with the ID '{PropertyValue}' not found."); ;
    }
    
    public static IRuleBuilderOptions<T, string?> MustBeAnExistingEntityAsync<T, TEntity>(
        this IRuleBuilderOptions<T, string?> builder,
        IReadOnlyRepository repository) where TEntity : Entity
    {
        return builder.MustAsync(async (entity, id, context, cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var entityGuid))
            {
                return false;
            }

            context.MessageFormatter.AppendArgument("EntityName", typeof(TEntity).Name);
            return await repository.ExistsAsync<TEntity>(entityGuid, cancellationToken);
        })
        .WithMessage("{EntityName} with the ID '{PropertyValue}' not found.");
    }
}
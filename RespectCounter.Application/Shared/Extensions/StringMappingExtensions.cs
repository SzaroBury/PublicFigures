using System.Globalization;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Shared.Extensions;

public static class StringMappingExtensions
{
    public static ActivitySortBy ToActivitySortByEnum(this string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return ActivitySortBy.Trending;
        }

        if (!Enum.TryParse<ActivitySortBy>(order, true, out var sortBy))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<ActivitySortBy>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return sortBy;
    }

    public static ActivityType ToActivityTypeEnum(this string typeInput)
    {
        // if (string.IsNullOrWhiteSpace(typeInput))
        // {
        //     return null;
        // }

        if (!Enum.TryParse<ActivityType>(typeInput, true, out var type))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<ActivityType>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return type;
    }

    public static PersonSortBy ToPersonSortByEnum(this string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return PersonSortBy.Trending;
        }

        if (!Enum.TryParse<PersonSortBy>(order, true, out var sortBy))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<PersonSortBy>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return sortBy;
    }

    public static CommentSortBy ToCommentSortByEnum(this string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return CommentSortBy.MostRespected;
        }

        if (!Enum.TryParse<CommentSortBy>(order, true, out var sortBy))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<CommentSortBy>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return sortBy;
    }

    public static DateTime ToDateTime(this string dateTimeString)
    {
        if (DateTime.TryParse(dateTimeString, out var dateTime))
        {
            throw new FormatException("Invalid DateTime format");
        }

        return dateTime;
    }

    public static DateOnly ToDateOnly(this string dateOnlyString)
    {
        return DateOnly.ParseExact(dateOnlyString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public static DateTime? ToNullableDateTime(this string? dateTimeString)
    {
        if (string.IsNullOrWhiteSpace(dateTimeString))
        {
            return null;
        }

        if (DateTime.TryParse(dateTimeString, out var dateTime))
        {
            return dateTime;
        }

        return null;
    }

    public static Guid ToGuid(this string guid)
    {
        return Guid.Parse(guid);
    }
    
    public static Guid? ToNullableGuid(this string? id)
    {
        if (id is null) return null;

        if (!Guid.TryParse(id, out Guid guid))
        {
            return null;
        }
        return guid;
    }
}
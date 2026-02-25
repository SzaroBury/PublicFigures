using System.Security.Claims;
using System.Text;
using RespectCounter.Application.Shared.DTOs;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainComment = RespectCounter.Domain.Model.Comment;
using DomainPerson = RespectCounter.Domain.Model.Person;

namespace RespectCounter.Application.Shared.Extensions;

public static class DtoMappingExtensions
{
    private static readonly string _defaultAvatarUrl = "default.jpg"; //to move to appsettings

    public static ActivityDTO ToDTO(this DomainActivity a, Guid? userGuid = null)
    {
        return new ActivityDTO(
            a.Id.ToString(),
            a.Person?.Id.ToString() ?? "??",
            $"{a.Person?.FirstName ?? "?"} {a.Person?.LastName ?? "?"}",
            a.Person?.Reactions?.Sum(r => (int)r.ReactionType) ?? 0,
            a.Person?.AvatarUrl ?? _defaultAvatarUrl,
            a.Status.ToString(),
            a.CreatedBy?.Username ?? "??",
            a.CreatedById.ToString(),
            a.Value,
            a.Description,
            a.Location,
            a.Source,
            string.Join(",", a.Tags.Select(pt => pt.Tag.Name)),
            a.Happend?.ToString("o") ?? "",
            a.Comments.Count + a.Comments.Sum(c => c.ChildrenCount),
            (int)a.Type,
            a.Reactions.Sum(r => (int)r.ReactionType),
            a.Reactions.ToList().Find(r => r.CreatedById == userGuid)?.ReactionType
        );
    } 

    public static PersonDTO ToDTO(this DomainPerson p, Guid? userGuid)
    {
        return new PersonDTO(
            p.Id.ToString(),
            p.FirstName,
            p.LastName,
            p.NickName,
            $"{p.FirstName} {p.LastName}" + (string.IsNullOrEmpty(p.NickName) ? "" : $" ({p.NickName})"),
            p.Profession,
            p.Description,
            p.AvatarUrl ?? _defaultAvatarUrl,
            p.Nationality,
            p.Birthday?.ToString() ?? "",
            p.DeathDate?.ToString() ?? "",
            p.Status.ToString(),
            p.CreatedBy?.Username ?? "??",
            p.CreatedById.ToString(),
            p.Tags.OrderByDescending(pt => pt.Tag.Count).Select(t => t.Tag.ToSimpleDTO()).ToList(),
            p.Activities.Count,
            p.Comments.Count,
            p.Reactions.Sum(r => (int)r.ReactionType),
            p.Reactions.ToList().Find(r => r.CreatedById == userGuid)?.ReactionType
        );
    }

    public static CommentDTO ToDTO(this DomainComment c, int levelsToSearch, Guid? userGuid = null)
    {
        return new CommentDTO(
            c.Id.ToString(),
            c.CreatedBy?.Username ?? "??",
            c.CreatedById.ToString(),
            c.CreatedBy?.AvatarUrl ?? _defaultAvatarUrl,
            c.Created.ToString("o"),
            c.Content, 
            c.ActivityId?.ToString() ?? "",
            c.PersonId?.ToString() ?? "",
            c.ParentId?.ToString() ?? "",
            (int)c.Status,
            c.Reactions.Sum(r => (int)r.ReactionType),
            c.ChildrenCount,
            levelsToSearch > 0 ? 
                c.Children.OrderByDescending(ch => ch.Created).Select(ch => ch.ToDTO(levelsToSearch - 1, userGuid)).ToList() 
                : [],
            c.Reactions.ToList().Find(r => r.CreatedById == userGuid)?.ReactionType
        );
    }

    public static TagDTO ToDTO(this Domain.Model.Tag tag)
    {
        return new TagDTO(
            tag.Name, 
            tag.Description, 
            tag.Activities.Count, 
            tag.Persons.Count, 
            tag.Count
        );
    }

    public static ClaimDTO ToDTO(this Claim claim)
    {
        return new ClaimDTO(
            claim.Type, 
            claim.Value
        );
    }

    public static SimplePersonDTO ToSimpleDTO(this DomainPerson p)
    {
        var nickNameSB = new StringBuilder($"{p.FirstName} {p.LastName}");
        if(!string.IsNullOrEmpty(p.NickName))
        {
            nickNameSB.Append($" ({p.NickName})");
        }

        return new SimplePersonDTO(
            p.Id.ToString(),
            nickNameSB.ToString()
        );
    }

    public static SimpleTagDTO ToSimpleDTO(this Domain.Model.Tag t)
    {
        return new SimpleTagDTO(
            t.Id.ToString(),
            t.Name
        );
    }
}
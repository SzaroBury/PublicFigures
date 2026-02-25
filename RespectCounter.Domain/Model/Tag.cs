using System.ComponentModel.DataAnnotations;

namespace RespectCounter.Domain.Model;

public class Tag : Entity
{
    public Tag(): base() { }
    public Tag(User user, DateTime? now) : base(user, now) {}
    public Tag(Guid userId, DateTime? now) : base(userId, now) {}

    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<PersonTag> Persons { get; init; } = [];
    public virtual ICollection<ActivityTag> Activities { get; init; } = [];
    public int CountActivities => Activities.Count;
    public int CountPersons => Persons.Count;
    public int Count => CountActivities + CountPersons;
}
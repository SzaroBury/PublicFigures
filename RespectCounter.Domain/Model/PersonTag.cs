namespace RespectCounter.Domain.Model;

public class PersonTag : Auditable
{
    public PersonTag(): base() { }
    public PersonTag(Person person, Tag tag, User user, DateTime now) : base(user, now)
    {
        PersonId = person.Id;
        Person = person;
        TagId = tag.Id;
        Tag = tag;
    }

    public PersonTag(Guid personId, Guid tagId, Guid userId, DateTime now) : base(userId, now)
    {
        PersonId = personId;
        TagId = tagId;
    }

    public Guid PersonId { get; init; }
    public virtual Person Person { get; init; } = null!;
    public Guid TagId { get; init; }
    public virtual Tag Tag { get; init; } = null!;
}
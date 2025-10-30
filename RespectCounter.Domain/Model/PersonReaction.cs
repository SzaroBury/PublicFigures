using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public class PersonReaction : BaseReaction
{
    public PersonReaction() : base() { }
    public PersonReaction(Person person, ReactionType type, User user, DateTime now) : base(type, user, now)
    {
        PersonId = person.Id;
        Person = person;
    }

    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;
}
namespace RespectCounter.Domain.Model;

public abstract class Entity : Auditable
{
    public Entity() { }
    public Entity(User user, DateTime? now) : base(user, now) {}
    public Entity(Guid userId, DateTime? now) : base(userId, now) {}

    public Guid Id { get; init; } = Guid.NewGuid();
    public bool Deleted { get; private set; } = false;

    public virtual void SoftDelete(User user, DateTime? now)
    {
        if (!now.HasValue) now = DateTime.UtcNow;
        Deleted = true;
        Updated(user, now);
    }
}   

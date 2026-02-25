namespace RespectCounter.Domain.Model;

public abstract class Auditable
{
    public Auditable() {}

    public Auditable(User user, DateTime? now)
    {
        if (!now.HasValue) now = DateTime.UtcNow;
        CreatedBy = user;
        CreatedById = user.Id;
        Created = now.Value;
        LastUpdatedBy = user;
        LastUpdatedById = user.Id;
        LastUpdated = now.Value;
    }

    public Auditable(Guid userId, DateTime? now)
    {
        if (!now.HasValue) now = DateTime.UtcNow;
        CreatedById = userId;
        Created = now.Value;
        LastUpdatedById = userId;
        LastUpdated = now.Value;
    }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public Guid CreatedById { get; set; } = Guid.Empty;
    public virtual User CreatedBy { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public Guid LastUpdatedById { get; set; } = Guid.Empty;
    public virtual User LastUpdatedBy { get; set; } = null!;

        public virtual void Updated(User user, DateTime? now)
    {
        if (!now.HasValue) now = DateTime.UtcNow;
        LastUpdatedBy = user;
        LastUpdatedById = user.Id;
        LastUpdated = now.Value;
    }
}
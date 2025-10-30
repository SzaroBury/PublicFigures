namespace RespectCounter.Domain.Model;

public class User : Entity
{
    public User() : base() { }
    public User(DateTime? now)
    {
        DateTime newNow = DateTime.UtcNow;
        Created = now ?? newNow;
        LastUpdated = now ?? newNow;
    }

    public string Username { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public virtual ICollection<Tag> RecentlyBrowsedTags { get; set; } = [];
    public virtual ICollection<Tag> FavoriteTags { get; set; } = [];
}
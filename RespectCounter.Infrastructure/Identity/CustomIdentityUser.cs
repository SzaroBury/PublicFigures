using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Identity;

public class CustomIdentityUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public virtual User? Profile { get; set; }
}
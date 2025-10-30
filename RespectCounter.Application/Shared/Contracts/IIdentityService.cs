using RespectCounter.Application.Shared.DTOs;

namespace RespectCounter.Application.Shared.Contracts;

public interface IIdentityService
{
    Task<IdentityDTO?> FindIdentityAsync(string identifier, CancellationToken cancellationToken);
    Task<IdentityDTO?> FindIdentityByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid identityId, CancellationToken cancellationToken);
    Task<bool> ExistsByUsernameOrEmailAsync(string identifier, CancellationToken cancellationToken);
    Task<bool> IsUniqueUsernameAsync(string username, CancellationToken cancellationToken);
    Task<bool> IsUniqueEmailAdressAsync(string email, CancellationToken cancellationToken);

    Task<bool> CheckPasswordAsync(string username, string password);
    Task<bool> IsInRoleAsync(Guid identityId, string roleName, CancellationToken cancellationToken);
    Task<IdentityDTO> CreateAsync(string userName, string password, string? email);
    Task SetRefreshTokenAsync(Guid identityId, string? refreshToken, DateTime? refreshTokenExpiration);
    Task<AuthTokensDTO> GenerateAndSetAuthTokensAsync(IdentityDTO identity, CancellationToken cancellationToken);
}
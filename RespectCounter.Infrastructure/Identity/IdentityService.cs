using System.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;

namespace RespectCounter.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly IJwtService _jwtService;

    public IdentityService(UserManager<CustomIdentityUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<Guid> GetGuidAsync(string username, CancellationToken cancellationToken)
    {
        var identity = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == username, cancellationToken);
        if (identity is not null)
        {
            identity = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == username, cancellationToken);
        }

        if (identity is null)
        {
            throw new KeyNotFoundException("User does not exist.");
        }

        return identity.Id;
    }

    public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userManager.Users.Where(u => u.Id == userId).AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameOrEmailAsync(string identifier, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == identifier, cancellationToken);
        if (user is not null)
        {
            return true;
        }

        user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == identifier, cancellationToken);
        if (user is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsUniqueEmailAdressAsync(string email, CancellationToken cancellationToken)
    {
        return !await _userManager.Users.Where(u => u.NormalizedEmail == email).AnyAsync(cancellationToken);
    }

    public async Task<bool> IsUniqueUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return !await _userManager.Users.Where(u => u.NormalizedUserName == username).AnyAsync(cancellationToken);
    }

    public async Task<bool> CheckPasswordAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(username);
            if(user == null)
            {
                throw new SecurityException($"{username} was not found.");
            }
        }  
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(Guid identityId)
    {
        var identity = await GetByIdAsync(identityId);
        return await _userManager.GetRolesAsync(identity);
    }

    public async Task<bool> IsInRoleAsync(Guid identityId, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == identityId, cancellationToken)
            ?? throw new InvalidOperationException($"User with ID {identityId} was not found.");

        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<IdentityDTO> CreateAsync(string userName, string password, string? email) //to-do: implement cancellationToken
    {
        var identity = new CustomIdentityUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(identity, password); //to-do: implement cancellationToken

        if (!result.Succeeded)
        {
            throw new SecurityException(result.ToString());
        }

        var normalizedUsername = await _userManager.GetUserNameAsync(identity);
        var roles = await _userManager.GetRolesAsync(identity);

        if (string.IsNullOrWhiteSpace(normalizedUsername)) throw new Exception("The user has no username.");

        return new IdentityDTO(identity.Id, normalizedUsername, roles);
    }

    public async Task SetRefreshTokenAsync(Guid identityId, string? refreshToken, DateTime? refreshTokenExpiration)
    {
        var user = await GetByIdAsync(identityId);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = refreshTokenExpiration;

        await _userManager.UpdateAsync(user);
    }

    public async Task<AuthTokensDTO> GenerateAndSetAuthTokensAsync(IdentityDTO identity, CancellationToken cancellationToken)
    {
        (string accessToken, DateTime accessTokenExpiration) = _jwtService.GenerateAccessToken(identity.Id.ToString(), identity.Username, identity.Roles);
        (string refreshToken, DateTime refreshTokenExpiration) = _jwtService.GenerateRefreshToken();
        await SetRefreshTokenAsync(identity.Id, refreshToken, refreshTokenExpiration);

        return new AuthTokensDTO(accessToken, accessTokenExpiration, refreshToken, refreshTokenExpiration);
    }

    public async Task<IdentityDTO?> FindIdentityByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var identity = await _userManager.Users.FirstOrDefaultAsync(i => i.RefreshToken == refreshToken && i.RefreshTokenExpiration > DateTime.UtcNow);

        if (identity is null) return null;
        if (identity.NormalizedUserName is null) throw new Exception("The user doesn't have any username.");

        var roles = await _userManager.GetRolesAsync(identity);
        return new IdentityDTO(identity.Id, identity.NormalizedUserName, roles);
    }

    public async Task<IdentityDTO?> FindIdentityAsync(string identifier, CancellationToken cancellationToken)
    {
        var identity = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == identifier, cancellationToken);
        if (identity is null)
        {
            identity = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == identifier, cancellationToken);
        }

        if (identity is null) return null;
        if (identity.NormalizedUserName is null) throw new Exception("The user doesn't have any username.");

        var roles = await _userManager.GetRolesAsync(identity);
        return new IdentityDTO(identity.Id, identity.NormalizedUserName, roles);
    }

    private async Task<CustomIdentityUser> GetByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new KeyNotFoundException($"Identity with the given ID {id} not found.");
        return appUser;
    }

    // public async Task<CustomIdentityUser?> FindByIdAsync(Guid id)
    // {
    //     CustomIdentityUser? appUser = await _userManager.FindByIdAsync(id.ToString());

    //     if (appUser == null)
    //     {
    //         return null;
    //     }
        
    //     return appUser;
    // }

    // public async Task<CustomIdentityUser> GetByNameAsync(string username)
    // {
    //     var appUser = await _userManager.FindByNameAsync(username)
    //         ?? throw new KeyNotFoundException($"User with the given username ({username}) was not found.");
    //     return appUser;
    // }

    // public async Task<CustomIdentityUser?> FindByNameAsync(string username)
    // {
    //     CustomIdentityUser? appUser = await _userManager.FindByNameAsync(username);

    //     if (appUser == null)
    //     {
    //         return null;
    //     }
        
    //     return appUser;
    // }

    // public async Task<CustomIdentityUser> GetByEmailAsync(string email)
    // {
    //     var appUser = await _userManager.FindByEmailAsync(email)
    //         ?? throw new KeyNotFoundException($"User with the given email ({email}) was not found.");
    //     return appUser;
    // }

    // public async Task<CustomIdentityUser?> FindByEmailAsync(string email)
    // {
    //     CustomIdentityUser? appUser = await _userManager.FindByEmailAsync(email);

    //     if (appUser == null)
    //     {
    //         return null;
    //     }
        
    //     return appUser;
    // }

    
    // public async Task<CustomIdentityUser?> FindByIdentifierAsync(string identifier)
    // {
    //     var user = await _userManager.FindByNameAsync(identifier);
    //     if (user is null)
    //     {
    //         user = await _userManager.FindByEmailAsync(identifier);
    //     }
    //     return user?;
    // }

    // public async Task<CustomIdentityUser> GetByRefreshTokenAsync(string refreshToken)
    // {
    //     if (string.IsNullOrWhiteSpace(refreshToken))
    //     {
    //         throw new SecurityException("Invalid refreshToken");
    //     }

    //     var appUser = await _userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken)
    //         ?? throw new SecurityException("Invalid refreshToken");

    //     if (appUser.RefreshTokenExpiration.HasValue
    //         && appUser.RefreshTokenExpiration < DateTime.UtcNow)
    //     {
    //         appUser.RefreshToken = null;
    //         appUser.RefreshTokenExpiration = null;
    //     }

    //     return appUser;
    // }
}

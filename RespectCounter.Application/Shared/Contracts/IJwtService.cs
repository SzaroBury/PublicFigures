namespace RespectCounter.Application.Shared.Contracts;

public interface IJwtService
{
    (string accessToken, DateTime accessTokenExpiration) GenerateAccessToken(string userId, string username, IEnumerable<string>? roles);
    (string refreshToken, DateTime refreshTokenExpiration) GenerateRefreshToken(int size = 32);
}
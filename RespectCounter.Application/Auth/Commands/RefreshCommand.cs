using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;

namespace RespectCounter.Application.Auth.Commands;

public record RefreshCommand(string RefreshToken) : IRequest<AuthTokensDTO>;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthTokensDTO>
{
    private readonly IIdentityService _identityService;

    public RefreshCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthTokensDTO> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var identity = await _identityService.FindIdentityByRefreshTokenAsync(request.RefreshToken, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");
        
        var tokens = await _identityService.GenerateAndSetAuthTokensAsync(identity, cancellationToken);

        return tokens;
    }
}
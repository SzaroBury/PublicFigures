using System.Security;
using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;

namespace RespectCounter.Application.Auth.Commands;

public record LoginCommand(string Username, string Password) : IRequest<AuthTokensDTO>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthTokensDTO>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthTokensDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
    {        
        if (!await _identityService.CheckPasswordAsync(request.Username, request.Password))
        {
            throw new SecurityException("Incorrect password.");
        }

        var identity = await _identityService.FindIdentityAsync(request.Username, cancellationToken)
            ?? throw new InvalidOperationException($"The User '{request.Username}' was not found in the system, despite the previous validation check.");

        var tokens = await _identityService.GenerateAndSetAuthTokensAsync(identity, cancellationToken);

        return tokens;
    }
}
using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;

namespace RespectCounter.Application.Auth.Commands;

public record RegisterCommand(
    string Email,
    string Username,
    string Password,
    string ConfirmPassword
) : IRequest<AuthTokensDTO>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthTokensDTO>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthTokensDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var identity = await _identityService.CreateAsync(request.Username, request.Email, request.Password);

        var tokens = await _identityService.GenerateAndSetAuthTokensAsync(identity, cancellationToken);

        return tokens;
    }
}
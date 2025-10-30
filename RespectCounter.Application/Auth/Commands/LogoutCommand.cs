using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Auth.Commands;

public record LogoutCommand(string UserId) : IRequest;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        await _identityService.SetRefreshTokenAsync(userId, null, null);

        return;
    }
}
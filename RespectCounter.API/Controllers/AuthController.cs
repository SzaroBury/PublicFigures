using System.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Extensions;
using RespectCounter.API.Requests;
using RespectCounter.Application.Auth.Commands;

namespace RespectCounter.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ISender _mediator;


    public AuthController(ILogger<AuthController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation($"{DateTime.Now}: Register([RegisterRequest])");

        var command = new RegisterCommand(request.Email, request.Username, request.Password, request.ConfirmPassword);
        var tokens = await _mediator.Send(command);

        AppendSecureCookie("AccessToken", tokens.AccessToken, tokens.AccessTokenExpiration);
        AppendSecureCookie("RefreshToken", tokens.RefreshToken, tokens.RefreshTokenExpiration);

        return Ok("Tokens refreshed successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation($"{DateTime.Now}: Login([LoginRequest])");

        var command = new LoginCommand(request.Username, request.Password);
        var tokens = await _mediator.Send(command);

        AppendSecureCookie("AccessToken", tokens.AccessToken, tokens.AccessTokenExpiration);
        AppendSecureCookie("RefreshToken", tokens.RefreshToken, tokens.RefreshTokenExpiration);

        var userClaims = new
        {
            UserName = User.Identity?.Name,
            User.Identity?.IsAuthenticated,
            Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
        };
        return Ok(userClaims);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        _logger.LogInformation($"{DateTime.Now}: Logout()");

        var command = new LogoutCommand(User.GetCurrentUserId());
        await _mediator.Send(command);
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return Ok("Logout successful");
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        _logger.LogInformation($"{DateTime.Now}: Refresh()");

        var refreshToken = Request.Cookies["RefreshToken"]
            ?? throw new SecurityException("RefreshToken was not found in headers of the request.");
        var command = new RefreshCommand(refreshToken);
        var tokens = await _mediator.Send(command);

        AppendSecureCookie("AccessToken", tokens.AccessToken, tokens.AccessTokenExpiration);
        AppendSecureCookie("RefreshToken", tokens.RefreshToken, tokens.RefreshTokenExpiration);

        return Ok("User registered successfully");
    }

    [HttpGet("claims")]
    [Authorize]
    public async Task<IActionResult> GetClaims()
    {
        _logger.LogInformation($"{DateTime.Now}: GetClaims()");
        var userClaims = new
        {
            UserName = User.Identity?.Name,
            User.Identity?.IsAuthenticated,
            Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
        };
        return Ok(await Task.FromResult(userClaims));
    }

    private void AppendSecureCookie(string key, string value, DateTime expiresDate)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresDate
        };
        Response.Cookies.Append(key, value, options);
    }
}
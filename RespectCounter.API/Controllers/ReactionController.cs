using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Extensions;
using RespectCounter.Application.Reactions.Commands;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/reactions")]
public class ReactionController : ControllerBase
{
    private readonly ILogger<ReactionController> _logger;
    private readonly ISender _mediator;

    public ReactionController(ILogger<ReactionController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Queries

    #endregion

    #region Commands
    [HttpPost("/api/activity/{id}/reaction/{reaction}")]
    [Authorize]
    public async Task<IActionResult> ReactionToActivity(string id, int reaction)
    {
        _logger.LogInformation($"{DateTime.Now}: ReactionToActivity(id: '{id}', reaction: {reaction})");
        var command = new AddReactionToActivityCommand(id, reaction, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/person/{id}/reaction/{reaction}")]
    [Authorize]
    public async Task<IActionResult> ReactionToPerson(string id, int reaction)
    {
        _logger.LogInformation($"{DateTime.Now}: ReactionToPerson(id: '{id}', reaction: {reaction})");
        var command = new AddReactionToPersonCommand(id, reaction, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);    }

    [HttpPost("/api/comment/{id}/reaction/{reaction}")]
    [Authorize]
    public async Task<IActionResult> ReactionToComment(string id, int reaction)
    {
        _logger.LogInformation($"{DateTime.Now}: ReactToComment(id: '{id}', reaction: {reaction})");
        var command = new AddReactionToCommentCommand(id, reaction, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }  

    #endregion
}
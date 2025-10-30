using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Extensions;
using RespectCounter.Application.Comment.Commands;
using RespectCounter.Application.Comment.Queries;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController: ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ISender _mediator;

    public CommentController(ILogger<CommentController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/activity/{id}/comments")]
    public async Task<IActionResult> GetCommentsForActivity(
        string id,
        int level = 2,
        string? order = "",
        int page = 1,
        int pageSize = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetCommentsForActivity(id: '{id}', level: {level})");
        var query = new GetCommentsForActivityQuery(
            id,
            level,
            page,
            pageSize,
            order,
            User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/person/{id}/comments")]
    public async Task<IActionResult> GetCommentsForPerson(
        string id,
        int level = 2,
        string? order = "",
        int page = 1,
        int pageSize = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetCommentsForPerson(id: '{id}', level: {level})");
        var query = new GetCommentsForPersonQuery(
            id,
            level,
            page,
            pageSize,
            order,
            User.TryGetCurrentUserId()
        );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    #endregion

    #region Commands
    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> CommentToComment(string id, [FromBody] string content)
    {
        _logger.LogInformation($"{DateTime.Now}: CommentToComment(id: '{id}', content: '{content}')");
        var command = new AddCommentToParentCommentCommand(id, content, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/activity/{id}/comment")]
    [Authorize]
    public async Task<IActionResult> CommentActivity(string id, [FromBody] string content)
    {
        _logger.LogInformation($"{DateTime.Now}: CommentActivity(id: '{id}', content: '{content}')");
        var command = new AddCommentToActivityCommand(id, content, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/person/{id}/comment")]
    [Authorize]
    public async Task<IActionResult> CommentPerson(string id, [FromBody] string content)
    {
        _logger.LogInformation($"{DateTime.Now}: CommentPerson(id: '{id}', content: '{content}')");
        var command = new AddCommentToPersonCommand(id, content, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateComment(string id, [FromBody] string content)
    {
        _logger.LogInformation($"{DateTime.Now}: UpdateComment(id: '{id}', content: '{content}')");
        var userId = User.GetCurrentUserId();
        var command = new UpdateCommentCommand(id, content, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}/hide")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> HideComment(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: HideComment(id: '{id}')");
        var userId = User.GetCurrentUserId();
        var command = new HideCommentCommand(id, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    #endregion
}
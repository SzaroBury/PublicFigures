using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Extensions;
using RespectCounter.Application.Tag.Commands;
using RespectCounter.Application.Tag.Queries;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/tag")]
public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;
    private readonly ISender _mediator;

    public TagController(ILogger<TagController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/tags")]
    public async Task<IActionResult> GetTags(int maxLevel = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetTags()");
        var query = new GetTagsQuery(maxLevel);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/tags/simple")]
    public async Task<IActionResult> GetSimpleTags()
    {
        _logger.LogInformation($"{DateTime.Now}: GetSimpleTags()");
        var query = new GetSimpleTagsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/tags/recent")]
    public IActionResult GetRecentlyBrowsedTags()
    {
        _logger.LogInformation($"{DateTime.Now}: GetRecentlyBrowsedTags()");
        // var query = new GetRecentlyBrowsedTagsQuery();
        // var result = await mediator.Send(query);
        // throw new NotImplementedException();
        return Ok(new List<Tag>());
    }

    [HttpGet("/api/tags/favourite")]
    public IActionResult GetFavouriteTags()
    {
        _logger.LogInformation($"{DateTime.Now}: GetFavouriteTags()");
        // var query = new GetFavouriteTagsQuery();
        // var result = await mediator.Send(query);
        // throw new NotImplementedException();
        return Ok(new List<Tag>());
    }

    [HttpGet("/api/person/{id}/tags")]
    public async Task<IActionResult> GetPersonTags(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: GetPersonTags(id: '{id}')");
        var query = new GetPersonTagsQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    #endregion

    #region Commands

    [HttpPost("/api/activity/{id}/tag/{tag}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TagActivity(string id, string tag)
    {
        _logger.LogInformation($"{DateTime.Now}: TagActivity(id: '{id}', tag: '{tag}')");
        var command = new AddTagToActivityCommand(id, tag, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/person/{id}/tag/{tag}")]
    [Authorize]
    public async Task<IActionResult> TagPerson(string id, string tag)
    {
        _logger.LogInformation($"{DateTime.Now}: TagPerson(id: '{id}', tag: '{tag}')");
        var command = new AddTagToPersonCommand(id, tag, User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    #endregion
}
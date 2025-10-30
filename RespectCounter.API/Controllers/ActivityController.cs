using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using MediatR;

using RespectCounter.Domain.Model;
using RespectCounter.API.Requests;
using RespectCounter.API.Mappers;
using RespectCounter.API.Extensions;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Activity.Queries;
using RespectCounter.Application.Activity.Commands;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/activity")]
public class ActivityController: ControllerBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly ISender _mediator;

    public ActivityController(ILogger<ActivityController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/activities/all")]
    public async Task<IActionResult> GetActivities(
        [FromQuery] string search = "",
        [FromQuery] string order = "",
        [FromQuery] string tags = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetActivities(search = '{search}', order = '{order}', tags = '{tags}')");
        var query = new GetActivitiesQuery(
            search,
            null,
            null,
            tags,
            [ActivityStatus.Verified, ActivityStatus.NotVerified],
            order,
            page,
            pageSize,
            User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/activities")]
    public async Task<IActionResult> GetVerifiedActivities(
        [FromQuery] string search = "",
        [FromQuery] string order = "",
        [FromQuery] string tags = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetVerifiedActivities(search = '{search}', order = '{order}', tags = '{tags}')");
        var query = new GetActivitiesQuery(
            search,
            null,
            null,
            tags,
            [ActivityStatus.Verified],
            order,
            page,
            pageSize,
            User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/person/{personId}/activities")]
    public async Task<IActionResult> GetActivitiesByPerson(
        string personId,
        string type = "",
        string order = "",
        bool? onlyVerified = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetActivitiesByPerson(personId = '{personId}', type = '{type}', order = '{order}', onlyVerified = '{onlyVerified}')");
        var query = new GetActivitiesQuery(
            "",
            personId,
            type,
            "",
            onlyVerified.ToActivityStatusHashSet(),
            order,
            page,
            pageSize,
            User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: GetActivity(id = '{id}')");

        var query = new GetActivityByIdQuery(id, User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    #endregion
    
    #region Commands
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProposeActivity(ProposeActivityRequest newActivity)
    {
        _logger.LogInformation($"{DateTime.Now}: ProposeActivity([ProposeActivityRequest])");
        var userId = User.GetCurrentUserId(); 
        var command = newActivity.ToAddCommand(userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}/verify")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VerifyActivity(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: VerifyActivity(id: '{id}')");
        var currentUserId = User.GetCurrentUserId(); 
        var command = new VerifyActivityCommand(id, currentUserId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public Task<IActionResult> ProposeUpdateActivity(string id, [FromBody] Activity activity)
    {
        _logger.LogInformation($"{DateTime.Now}: ProposeUpdateActivity(id: '{id}', [Activity])");
        throw new NotImplementedException();
    }

    [HttpPut("{id}/update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateActivity(string id, [FromBody] ProposeActivityRequest activity)
    {
        _logger.LogInformation($"{DateTime.Now}: UpdateActivity(id: '{id}', [Activity])");
        var userId = User.GetCurrentUserId();
        var command = activity.ToUpdateCommand(id, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}/hide")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> HideActivity(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: HideActivity(id: '{id}')");
        var currentUserId = User.GetCurrentUserId();
        var command = new HideActivityCommand(id, currentUserId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    #endregion
    
}
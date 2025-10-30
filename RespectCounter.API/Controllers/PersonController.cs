using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Extensions;
using RespectCounter.API.Mappers;
using RespectCounter.API.Requests;
using RespectCounter.Application.Person.Commands;
using RespectCounter.Application.Person.Queries;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController: ControllerBase
{
    private readonly ILogger<PersonController> _logger;
    private readonly ISender _mediator;

    public PersonController(ILogger<PersonController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/persons")]
    public async Task<IActionResult> GetVerifiedPersons(
        [FromQuery] string search = "",
        string tags = "",
        [FromQuery] string order = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {        
        _logger.LogInformation($"{DateTime.Now}: GetVerifiedPersons(search: '{search}', order: '{order}')");
        var query = new GetPersonsQuery(
            search,
            tags,
            [PersonStatus.Verified],
            order,
            page,
            pageSize,
            User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/persons/all")]
    public async Task<IActionResult> GetPersons(
        [FromQuery] string search = "",
        string tags = "",
        [FromQuery] string order = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation($"{DateTime.Now}: GetPersons(search: '{search}', order: '{order}')");
        var query = new GetPersonsQuery(
            search,
            tags,
            [PersonStatus.Verified, PersonStatus.NotVerified],
            order,
            page,
            pageSize,
            User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: GetPerson(id: '{id}')");
        var query = new GetPersonByIdQuery(id, User.TryGetCurrentUserId());
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/persons/names")]
    public async Task<IActionResult> GetSimplePersons()
    {
        _logger.LogInformation($"{DateTime.Now}: GetSimplePersons()");
        var query = new GetSimplePersonsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    #endregion

    #region Commands
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProposePerson(ProposePersonRequest request)
    {
        _logger.LogInformation($"{DateTime.Now}: ProposePerson({request})");
        var command = request.ToAddCommand(User.GetCurrentUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/verify")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VerifyPerson(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: VerifyPerson(id: '{id}')");
        var command = new VerifyPersonCommand(id, User.GetCurrentUserId());
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> ProposeUpdatePerson(string id, ProposePersonRequest request)
    {
        _logger.LogInformation($"{DateTime.Now}: ProposeUpdatePerson(id: '{id}', [Person])");
        throw new NotImplementedException();
    }

    [HttpPut("{id}/update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePerson(string id, ProposePersonRequest request)
    {
        _logger.LogInformation($"{DateTime.Now}: ProposeUpdatePerson(id: '{id}', [Person])");
        var userId = User.GetCurrentUserId();
        var command = request.ToUpdateCommand(id, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}/hide")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> HidePerson(string id)
    {
        _logger.LogInformation($"{DateTime.Now}: HidePerson(id: '{id}')");
        var userId = User.GetCurrentUserId();
        var command = new HidePersonCommand(id, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    #endregion
}
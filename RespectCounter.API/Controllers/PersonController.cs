using MediatR;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Queries;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

//Get Persons by
//  Search: Tags, Firstname, Lastname, Birth date, Nationality, Respect, Verified/NotVerified
//      Sorting: Best Match, Most Respect, Least Respect, Trending (last 7d reactions and respect), Alfpha
//  Activity Id
//  ?IdentityUserUid
//  Pages/Autoload more?
//Get Person by Id

//Post a person
//Verify a person
//React
//Comment
//Add a tag
//Propose a change
//?Hide a person/Mark as invalid

//other todos: auto added tag based on nationality

[ApiController]
[Route("api/persons")]
public class PersonController: ControllerBase
{

    private readonly ILogger<PersonController> logger;
    private readonly ISender mediator;

    public PersonController(ILogger<PersonController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Querries
    [HttpGet("/api/persons")]
    public async Task<IActionResult> GetVerifiedPersons([FromQuery] string search, [FromQuery] string order)
    {
        var query = new GetVerifiedPersonsQuery(search, order);
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/persons/all")]
    public async Task<IActionResult> GetPersons([FromQuery] string search, [FromQuery] string order)
    {
        var query = new GetPersonsQuery(search, order);
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(string id)
    {
        var query = new GetPersonByIdQuery(id);

        try
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    #endregion

    #region Commands
    [HttpPost()]
    public Task<IActionResult> ProposePerson([FromBody] Person newPerson)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/verify")]
    public Task<IActionResult> VerifyPerson(string id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/respect/{reaction}")]
    public Task<IActionResult> RespectPerson(string id, int reaction)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/comment")]
    public Task<IActionResult> CommentPerson(string id, [FromBody] Comment newComment)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/tag/{tags}")]
    public Task<IActionResult> TagPerson(string id, string tags)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public Task<IActionResult> ProposeUpdatePerson(string id, [FromBody] Person person)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    public Task<IActionResult> HidePerson(string id)
    {
        throw new NotImplementedException();
    }
    #endregion
}
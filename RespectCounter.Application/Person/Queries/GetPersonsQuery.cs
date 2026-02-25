using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Queries;

public record GetPersonsQuery(
    string? Search,
    IEnumerable<string>? Tags,
    HashSet<PersonStatus>? Status,
    string? Order,
    int Page, 
    int PageSize,
    string? UserId
) : IRequest<PagedResult<PersonDTO>>;

public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, PagedResult<PersonDTO>>
{
    private readonly IPersonRepository _repository;
    private readonly IIdentityService _userService;

    public GetPersonsQueryHandler(IPersonRepository repository, IIdentityService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    public async Task<PagedResult<PersonDTO>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<PersonStatus> statusFilter;
        if (request.Status == null || request.Status.Count == 0)
        {
            statusFilter = [PersonStatus.Verified, PersonStatus.NotVerified];
        }
        else
        {
            statusFilter = request.Status!;
        }

        var sortBy = request.Order.ToPersonSortByEnum();
        var userId = request.UserId.ToNullableGuid();

        var result = await _repository.GetPagedAsync(
            sortBy, 
            request.Page, 
            request.PageSize, 
            statusFilter,
            request.Search?.ToLower(),
            request.Tags?.Select(t => t.Trim().ToLower()) ?? [],
            userId,
            cancellationToken
        );

        return new PagedResult<PersonDTO>()
        {
            Items = result.Items.Select(p => p.ToDTO(userId)),
            TotalItems = result.TotalItems,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
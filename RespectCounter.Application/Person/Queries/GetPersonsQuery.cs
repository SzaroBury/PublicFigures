using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Queries;

public record GetPersonsQuery(
    string? Search,
    string? Tags,
    HashSet<PersonStatus>? Status,
    string? Order,
    int Page, 
    int PageSize,
    string? UserId
) : IRequest<PagedResult<PersonDTO>>;

public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, PagedResult<PersonDTO>>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IIdentityService _userService;

    public GetPersonsQueryHandler(IReadOnlyRepository repository, IIdentityService userService)
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

        IQueryable<DomainPerson> query = _repository.FindQueryable<DomainPerson>(a => statusFilter.Contains(a.Status));

        if (!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(
                p => p.FirstName.ToLower().Contains(search)
                    || p.LastName.ToLower().Contains(search)
                    || p.Nationality.ToLower().Contains(search)
                    || p.Description.ToLower().Contains(search)
                    || p.Tags.Any(pt => pt.Tag.Name.ToLower().Contains(search))
            );
        }

        if (!string.IsNullOrEmpty(request.Tags))
        {
            var tags = request.Tags.ToLower().Split(',');
            query = query.Where(p => tags.All(t => p.Tags.Select(pt => pt.Tag.Name.ToLower()).Contains(t)));
        }

        var totalItems = query.Count();

        var allTagsQuery = query.SelectMany(p => p.Tags).Distinct();
        var allTags = await _repository.FindListAsync(allTagsQuery, ["Activities", "Persons"], null, cancellationToken);

        var sortBy = request.Order.ToPersonSortByEnum();
        var orderedQuery = query.ApplySorting(sortBy);
        orderedQuery = orderedQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        var persons = await _repository.FindListAsync(
            orderedQuery,
            ["Comments.Children", "Reactions", "Tags", "CreatedBy", "LastUpdatedBy"],
            null,
            cancellationToken
        );

        Guid? userId = request.UserId.ToNullableGuid();
        var result = new PagedResult<PersonDTO>
        {
            Items = persons.Select(p => p.ToDTO(userId)),
            TotalItems = totalItems,
            PageNumber = request.Page,
            PageSize = request.PageSize,
            RelatedTags = allTags.Select(personTag => personTag.Tag.ToDTO())
        };

        return result;
    }
}
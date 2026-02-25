using RespectCounter.Application.Shared.Enums;
using RespectCounter.Domain.Enums;
using DomainActivity = RespectCounter.Domain.Model.Activity;

namespace RespectCounter.Application.Shared.Contracts;

public interface IActivityRepository
{
    Task<PagedResult<DomainActivity>> FindPagedResultAsync(
        int page, 
        int pageSize, 
        ActivitySortBy sortBy, 
        IEnumerable<ActivityStatus> statuses,

        ActivityType? type = null,
        string? search = null,
        IEnumerable<string>? tags = null,
        Guid? personId = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default
    );
    Task<DomainActivity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Update(DomainActivity activity);
    void Add(DomainActivity activity);
}
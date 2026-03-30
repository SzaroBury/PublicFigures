using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Tag.Queries;

public record GetPersonTagsQuery(string PersonId, int AtLeastCount = 1) : IRequest<IEnumerable<SimpleTagDTO>>;

public class GetPersonTagsQueryHandler : IRequestHandler<GetPersonTagsQuery, IEnumerable<SimpleTagDTO>>
{
    private readonly ITagRepository _repository;
    
    public GetPersonTagsQueryHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SimpleTagDTO>> Handle(GetPersonTagsQuery request, CancellationToken cancellationToken)
    {
        Guid personGuid = request.PersonId.ToGuid();
        var tags = await _repository.GetPersonTagsWithoutTrackingAsync(personGuid, 1, cancellationToken);
        return tags.Select(pt => pt.ToSimpleDTO());
    }
}
using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Tag.Queries
{
    public record GetSimpleTagsQuery() : IRequest<IEnumerable<SimpleTagDTO>>;

    public class GetSimpleTagsQueryHandler : IRequestHandler<GetSimpleTagsQuery, IEnumerable<SimpleTagDTO>>
    {
        private readonly ITagRepository _repository;

        public GetSimpleTagsQueryHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SimpleTagDTO>> Handle(GetSimpleTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repository.GetAllWithoutTrackingAsync(cancellationToken: cancellationToken);
            return tags.Select(t => t.ToSimpleDTO());
        }
    }
}
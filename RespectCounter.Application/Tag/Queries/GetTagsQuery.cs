using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Tag.Queries
{
    public record GetTagsQuery(int AtLeastCount = 1) : IRequest<IEnumerable<TagDTO>>;

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<TagDTO>>
    {
        private readonly ITagRepository _repository;

        public GetTagsQueryHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TagDTO>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {               
            var tags = await _repository.GetAllWithoutTrackingAsync(request.AtLeastCount, cancellationToken);
            return tags.Select(p => p.ToDTO());
        }
    }
}
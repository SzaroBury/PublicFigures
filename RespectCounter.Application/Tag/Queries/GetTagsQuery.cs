using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Tags.Queries
{
    public record GetTagsQuery(int AtLeastCount = 1) : IRequest<IEnumerable<TagDTO>>;

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<TagDTO>>
    {
        private readonly IReadOnlyRepository _repository;

        public GetTagsQueryHandler(IReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TagDTO>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {               
            var tags = await _repository.FindListAsync<Tag>(
                t => t.Count < request.AtLeastCount,
                ["Activities", "Persons"],
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
            return tags.Select(p => p.ToDTO());
        }
    }
}
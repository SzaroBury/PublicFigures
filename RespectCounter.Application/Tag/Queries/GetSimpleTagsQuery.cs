using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Tags.Queries
{
    public record GetSimpleTagsQuery() : IRequest<IEnumerable<SimpleTagDTO>>;

    public class GetSimpleTagsQueryHandler : IRequestHandler<GetSimpleTagsQuery, IEnumerable<SimpleTagDTO>>
    {
        private readonly IReadOnlyRepository _repository;

        public GetSimpleTagsQueryHandler(IReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SimpleTagDTO>> Handle(GetSimpleTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repository.FindListAsync<Tag>(
                t => t.Count > 0,
                ["Activities", "Persons"],
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
            return tags.Select(t => t.ToSimpleDTO());
        }
    }
}
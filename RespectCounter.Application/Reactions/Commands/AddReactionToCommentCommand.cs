using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;
using DomainComment = RespectCounter.Domain.Model.Comment;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Reactions.Commands;

public record AddReactionToCommentCommand(
    string CommentId,
    int ReactionType,
    string UserId
) : IRequest<int>;

public class AddReactionToCommentCommandHandler : IRequestHandler<AddReactionToCommentCommand, int>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddReactionToCommentCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<int> Handle(AddReactionToCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        DateTime now = DateTime.UtcNow;

        var commentId = request.CommentId.ToGuid();
        DomainComment targetComment = await _repository.SingleOrDefaultAsync<DomainComment>(c => c.Id == commentId, "Reactions")
            ?? throw new InvalidOperationException($"The comment with ID {request.CommentId} was not found in the system, despite the previous validation check.");

        if (!Enum.IsDefined(typeof(ReactionType), request.ReactionType))
        {
            throw new ArgumentException("Invalid format of the reaction type.");
        }
        
        CommentReaction? reaction = _repository.FindQueryable<CommentReaction>(r => r.CommentId == commentId && r.CreatedById == user.Id).FirstOrDefault();
        if(reaction != null)
        {
            //throw new InvalidOperationException("This user has already reacted to this comment.");
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.LastUpdated = now;
            _uow.GetWriteRepository().Update(reaction);
        }
        else
        {
            reaction = new CommentReaction(targetComment, (ReactionType)request.ReactionType, user, now);
            targetComment.Reactions.Add(reaction);
        }

        await _uow.CommitAsync(cancellationToken);
        var result = targetComment.Reactions.Sum(r => (int)r.ReactionType);
        return result;
    }
}
using MediatR;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Reactions.Commands;

public record AddReactionToCommentCommand(
    string CommentId,
    int ReactionType,
    string UserId
) : IRequest<int>;

public class AddReactionToCommentCommandHandler : IRequestHandler<AddReactionToCommentCommand, int>
{
    private readonly IReadOnlyRepository _roRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddReactionToCommentCommandHandler(
        IReadOnlyRepository roRepository,
        ICommentRepository commentRepository, 
        IReactionRepository reactionRepository,
        IUnitOfWork uow, 
        IIdentityService userService)
    {
        _roRepository = roRepository;
        _commentRepository = commentRepository;
        _reactionRepository = reactionRepository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<int> Handle(AddReactionToCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _roRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        DateTime now = DateTime.UtcNow;

        var commentId = request.CommentId.ToGuid();
        DomainComment targetComment = await _commentRepository.GetCommentByIdAsync(commentId, cancellationToken)
            ?? throw new InvalidOperationException($"The comment with ID {request.CommentId} was not found in the system, despite the previous validation check.");

        if (!Enum.IsDefined(typeof(ReactionType), request.ReactionType))
        {
            throw new ArgumentException("Invalid format of the reaction type.");
        }
        
        CommentReaction? reaction = await _reactionRepository.TryGetCurrentUserReactionForCommentAsync(commentId, user.Id);
        if(reaction != null)
        {
            //throw new InvalidOperationException("This user has already reacted to this comment.");
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.Updated(user, now);
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
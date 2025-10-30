using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IReactionable<T> where T : BaseReaction
{
    public ICollection<T> Reactions { get; init; }
}
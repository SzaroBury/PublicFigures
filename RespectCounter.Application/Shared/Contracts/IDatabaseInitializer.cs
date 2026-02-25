namespace RespectCounter.Application.Shared.Contracts;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}
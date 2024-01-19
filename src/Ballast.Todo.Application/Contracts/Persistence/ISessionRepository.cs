using Ballast.Todo.Domain.Entities;

namespace Ballast.Todo.Application.Contracts.Persistence
{
    public interface ISessionRepository
    {
        Task<bool> DeleteSessionsAsync(string userId);
        Task CreateSessionAsync(Session session);
    }
}
